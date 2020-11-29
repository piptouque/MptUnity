using System.Collections.Generic;
using System.Collections;

using UnityEngine.Events;

namespace MptUnity.Audio.Behaviour
{
    using AudioFile = UnityEngine.Object;
    
    public abstract class AbstractJukebox<TMusic> : UnityEngine.MonoBehaviour, IJukebox
        where TMusic : IMusic
    {
        #region Serialised 

        

        public AudioFile[] audioFiles;
        
        public bool shouldLoop;
        // Seconds to wait for the notes to die before completely stopping the playback, section.
        public float dieTimeSeconds;
        
        #endregion
        


        #region Life-cycle

        protected AbstractJukebox()
        {
            m_events = new Events();
            
            m_musicList = new List<TMusic>();
            
            m_currentMusicIndex = -1;
            
        }

        #endregion
        
        #region Unity MonoBehaviour event functions
        
        
        void Awake()
        {
            m_source = GetComponent<UnityEngine.AudioSource>();
            // In order to use our procedural filter as the input to the AudioSource,
            // we need to detach any potential AudioClip from it.
            // see: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAudioFilterRead.html
            m_source.clip = null;
            // must loop over the buffer, since it is refilled at each step.
            m_source.loop = true;
            m_source.Stop();

            m_sampleRate = MusicConfig.GetSampleRate();
        }
        
        void Start()
        {
            // We have no insurance that the manager will be ready if we try to load the file
            // in Awake, so we do it in Start instead.
            // This however requires that we check whether the player is ready
            // before calling Restart, Resume, Pause or Stop.
            foreach (var audioFile in audioFiles)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(audioFile);

                Load(path);
            }

            if (m_musicList.Count > 0)
            {
                SwitchMusic(0);
            }

        }

        void Update()
        {
            m_sampleRate = MusicConfig.GetSampleRate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback">Function to be called, takes no parameter.</param>
        /// <param name="owner">Caller of the method.</param>
        /// <param name="seconds">Seconds to wait, in Unity scaled time.</param>
        /// <returns></returns>
        static IEnumerator DelayedCoroutine(System.Action callback, AbstractJukebox<TMusic> owner, float seconds)
        {
            //Print the time of when the function is first called.
            UnityEngine.Debug.Log("Started Coroutine at timestamp : " + UnityEngine.Time.time);

            yield return new UnityEngine.WaitForSeconds(seconds);
            UnityEngine.Debug.Log("Finished Coroutine at timestamp : " + UnityEngine.Time.time);

            if (!(owner is null))
            {
                callback();
            }
        }

        #endregion
        
        
        #region IPlayable

        

        public bool IsPaused()
        {
            return GetPlaybackState() == AudioPlaybackState.ePaused;
        }

        public bool IsPlaying()
        {
            return GetPlaybackState() == AudioPlaybackState.ePlaying;
        }

        public bool IsStopped()
        {
            return GetPlaybackState() == AudioPlaybackState.eStopped;
        }

        public void Pause()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            SetPlaybackState(AudioPlaybackState.ePaused);
        }

        public void Play()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            SetPlaybackState(AudioPlaybackState.ePlaying);
        }

        /// <summary>
        /// Stops playback after having stopped the notes!
        /// </summary>
        public void Stop()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            // Stop playback after dieTimeSeconds have passed.
            /*
            StartCoroutine(
                DelayedCoroutine(
                    () =>
                    {
                        SetPlaybackState(AudioPlaybackState.eStopped);
                    },
                    this,
                    dieTimeSeconds
                    )
                );
            */
            SetPlaybackState(AudioPlaybackState.eStopped);
        }

        #endregion

        #region Playback

        void SetPlaybackState(AudioPlaybackState state)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            
            SetStreamState(state, m_state);
            m_state = state;
            m_events.playbackChangeEvent.Invoke(m_state);
        }

    
        public AudioPlaybackState GetPlaybackState()
        {
            return m_state;
        }
        
        public void Unload()
        {
            for (int musicIndex = 0; musicIndex < GetLoadedNumber(); ++musicIndex)
            {
                UnloadOne(musicIndex);                 
            }
        }


        #endregion
        
        #region IMusicPlayer implementation

        public double GetPlayingTempo()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().GetPlayingTempo();
        }

        public double GetTempoFactor()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().GetTempoFactor();
        }

        public void SetTempoFactor(double tempo)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            GetCurrentMusic().SetTempoFactor(tempo);
            m_events.bpmChangeEvent.Invoke(GetPlayingTempo());
        }

        public void MoveTempoFactor(double rate)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            SetTempoFactor(GetTempoFactor() * (1D + rate));
        }

        public int NumberSections
        {
            get
            {
                UnityEngine.Assertions.Assert.IsTrue(IsReady());
                return GetCurrentMusic().NumberSections;
            }
        }
        
        public string GetTitle()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().GetTitle();
        }

        public string GetAuthor()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().GetAuthor();
        }

        public string GetComment()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().GetComment();
        }

        public MusicSection GetSection(int sectionIndex)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().GetSection(sectionIndex);
        }

        /// <summary>
        /// Wait for the notes to die before muting the section.
        /// </summary>
        /// <param name="sectionIndex"></param>
        /// <param name="mute"></param>
        public void MuteSection(int sectionIndex, bool mute)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            GetCurrentMusic().MuteSection(sectionIndex, mute);
            m_events.muteSectionChangeEvent.Invoke(
                GetCurrentMusic().GetSection(sectionIndex), mute
                );
        }

        public bool IsSectionMuted(int sectionIndex)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().IsSectionMuted(sectionIndex);
        }
        
        public void StopSectionNotes(int sectionIndex)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            GetCurrentMusic().StopSectionNotes(sectionIndex);
            
        }

        public void SetRepeatCount(int count)
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            GetCurrentMusic().SetRepeatCount(count);
        }

        public int GetRepeatCount()
        {
            UnityEngine.Assertions.Assert.IsTrue(IsReady());
            return GetCurrentMusic().GetRepeatCount();
        }

        public void SwitchMusic(int musicIndex)
        {
            UnityEngine.Assertions.Assert.IsTrue(musicIndex < GetLoadedNumber());
            UnityEngine.Assertions.Assert.IsTrue(musicIndex < GetLoadedNumber());

            // Reset previous music.
            // Only if there *is* some previous music,
            // that is, if the player is ready.
            if (IsReady())
            {
                GetCurrentMusic().Reset();
            }
            
            // Remember playback state.
            // So that GetCurrentMusic gets us the new music.
            m_currentMusicIndex = musicIndex;

            // If the player is set for looping, signal the new music.
            if (shouldLoop)
            {
                GetCurrentMusic().SetRepeatCount(-1);
            }
            

            // an Invoke a day keeps the bugs away!!
            m_events.musicSwitchEvent.Invoke(GetCurrentMusic());
            
        }

        public int GetLoadedNumber()
        {
            return m_musicList.Count;
        }
        
        public int GetCurrentMusicIndex()
        {
            return m_currentMusicIndex;
        }

        public bool IsReady()
        {
            return (m_currentMusicIndex >= 0)
                   && (m_currentMusicIndex < GetLoadedNumber());
        }
        #endregion // IMusicPlayer implementation


        #region Event handling 
        protected class Events
        {
            public readonly MusicLoadEvent      musicLoadEvent;
            public readonly MusicSwitchEvent    musicSwitchEvent;
            public readonly PlaybackChangeEvent playbackChangeEvent;
            public readonly BpmChangeEvent      bpmChangeEvent;
            public readonly MuteSectionChangeEvent muteSectionChangeEvent;
            
            public Events()
            {
                musicLoadEvent       = new MusicLoadEvent();
                musicSwitchEvent     = new MusicSwitchEvent();
                playbackChangeEvent  = new PlaybackChangeEvent();
                bpmChangeEvent       = new BpmChangeEvent();
                muteSectionChangeEvent  = new MuteSectionChangeEvent();
            }
            
        }

        public void AddMusicLoadListener(UnityAction<IMusic, bool> onMusicLoad)
        {
            m_events.musicLoadEvent.AddListener(onMusicLoad);
        }
        
        public void AddMusicSwitchListener(UnityAction<IMusic> onMusicSwitch)
        {
            m_events.musicSwitchEvent.AddListener(onMusicSwitch);
        }

        public void AddBpmChangeListener(UnityAction<double> onBpmChange)
        {
            m_events.bpmChangeEvent.AddListener(onBpmChange);
        }

        public void AddMuteSectionChangeListener(UnityAction<MusicSection, bool> onMuteSectionChange)
        {
            m_events.muteSectionChangeEvent.AddListener(onMuteSectionChange);
        }

        public void AddPlaybackChangeListener(UnityAction<AudioPlaybackState> onPlaybackChange)
        {
            m_events.playbackChangeEvent.AddListener(onPlaybackChange);
        }

        public void RemoveBpmChangeListener(UnityAction<double> onBpmChange)
        {
            m_events.bpmChangeEvent.RemoveListener(onBpmChange);
        }

        public void RemoveMuteSectionChangeListener(UnityAction<MusicSection, bool> onMuteSectionChange)
        {
            m_events.muteSectionChangeEvent.RemoveListener(onMuteSectionChange);
        }

        public void RemovePlaybackChangeListener(UnityAction<AudioPlaybackState> onPlaybackChange)
        {
            m_events.playbackChangeEvent.RemoveListener(onPlaybackChange);
        }

        public void RemoveMusicLoadListener(UnityAction<IMusic, bool> onMusicLoad)
        {
            m_events.musicLoadEvent.RemoveListener(onMusicLoad);
        }
        
        public void RemoveMusicSwitchListener(UnityAction<IMusic> onMusicSwitch)
        {
            m_events.musicSwitchEvent.RemoveListener(onMusicSwitch);
        }

        #endregion // Event Handling

        
        #region To resolve

        

        /// <summary>
        /// Update the internal streaming according to playback state.
        /// </summary>
        /// <param name="updated"></param>
        /// <param name="previous"></param>
        void SetStreamState(AudioPlaybackState updated, AudioPlaybackState previous)
        {
            switch (updated)
            {
               case AudioPlaybackState.ePaused:
                   m_source.Pause();
                   break;
               case AudioPlaybackState.ePlaying:
                   if (previous == AudioPlaybackState.eStopped)
                   {
                       m_source.Play();
                   }
                   else
                   {
                       m_source.UnPause();
                   }
                   break;
               case AudioPlaybackState.eStopped:
                   m_source.Stop();
                   break;
            }
        }
        
        protected abstract TMusic CreateMusic(string path);

        public abstract void OnAudioFilterRead(float[] data, int channels);

        #endregion

        #region Util 
        
        
        void Load(string path)
        {
            try
            {
                TMusic music = CreateMusic(path);
                m_musicList.Add(music);
                // time to notify the gang.
                m_events.musicLoadEvent.Invoke(music, true);
            }
            catch (System.ArgumentException)
            {
                UnityEngine.Debug.LogError(
                    "Failed to load MOD music."
                    ) ;
            }

        }
        
        protected TMusic GetCurrentMusic()
        {
            return m_musicList[m_currentMusicIndex];
        }
        


        void UnloadOne(int musicIndex)
        {
            UnityEngine.Assertions.Assert.IsTrue(musicIndex < GetLoadedNumber());
            
            m_musicList.RemoveAt(musicIndex);
        }

        #endregion
        
        #region Protected data 

        readonly List<TMusic> m_musicList;
        int m_currentMusicIndex;

        AudioPlaybackState m_state;
        
        UnityEngine.AudioSource m_source;
            
        protected readonly Events m_events;

        protected int m_lastBpm;

        protected int m_sampleRate;


        #endregion

    }

}