
using UnityEngine.Events;

namespace MptUnity.Audio.Behaviour
{
    
    public class PlaybackChangeEvent : UnityEvent<Audio.AudioPlaybackState> { }
    public class MusicLoadEvent : UnityEvent<IMusic, bool> { }
    public class MusicSwitchEvent : UnityEvent<IMusic>  {}
    public class BpmChangeEvent : UnityEvent<double> { }
    public class MuteSectionChangeEvent : UnityEvent<MusicSection, bool> { }
    
    public interface IJukebox : IPlayable, IMusicSource
    {

        int NumberSections { get;  }

        /// <summary>
        /// Get current tempo as modified by the tempo factor.
        /// </summary>
        /// <returns></returns>
        double GetPlayingTempo();
        /// <summary>
        /// Get current tempo modifier.
        /// </summary>
        /// <returns></returns>
        double GetTempoFactor();
        /// <summary>
        /// Set tempo modifier.
        /// </summary>
        /// <returns></returns>
        void SetTempoFactor(double factor);

        /// <summary>
        /// Multiplies current tempo factor by `factor * (1 + rate)`.
        /// </summary>
        /// <param name="rate"></param>
        void MoveTempoFactor(double rate);
        
        string GetTitle();
        string GetAuthor();
        string GetComment();

        MusicSection GetSection(int sectionIndex);
        
        void MuteSection(int sectionIndex, bool mute);

        bool IsSectionMuted(int sectionIndex);
        
        /// <summary>
        /// Stops all currently playing notes in the section at sectionIndex.
        /// </summary>
        /// <param name="sectionIndex"></param>
        void StopSectionNotes(int sectionIndex);


        /// <summary>
        /// Sets the current music repeat state.
        ///   -1 loops indefinitely.
        ///   0 plays once.
        ///   n > 0 plays once then repeats n times.
        /// </summary>
        /// <param name="count"></param>
        void SetRepeatCount(int count);
        int GetRepeatCount();

        /// <summary>
        /// Resets the currently played music, then switches playback to music loaded at index musicIndex.
        /// </summary>
        /// <param name="musicIndex"></param>
        void SwitchMusic(int musicIndex);
        int GetLoadedNumber();
        int GetCurrentMusicIndex();
        bool IsReady();

        void AddPlaybackChangeListener(UnityAction<AudioPlaybackState> onPlaybackChange);
        void AddMusicLoadListener(UnityAction<IMusic, bool> onMusicLoad);
        void AddMusicSwitchListener(UnityAction<IMusic> onMusicSwitch);
        void AddBpmChangeListener(UnityAction<double> onBpmChange);
        void AddMuteSectionChangeListener(UnityAction<MusicSection, bool> onMuteSectionChange);
        void RemovePlaybackChangeListener(UnityAction<AudioPlaybackState> onPlaybackChange);
        void RemoveMusicSwitchListener(UnityAction<IMusic> onMusicSwitch);
        void RemoveMusicLoadListener(UnityAction<IMusic, bool> onMusicLoad);
        void RemoveBpmChangeListener(UnityAction<double> onBpmChange);
        void RemoveMuteSectionChangeListener(UnityAction<MusicSection, bool> onMuteSectionChange);

    }
}