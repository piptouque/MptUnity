using MptUnity.Utility;

namespace MptUnity.Audio
{
    public class OpenMptInstrument : IInstrument
    {

        #region Life-cycle

        public OpenMptInstrument(string path, int numberVoices)
        {
            m_moduleExt = OpenMptUtility.LoadModuleExt(path);
            if (m_moduleExt == null)
            {
                throw new System.ArgumentException("Error when loading MOD music at path: " + path);
            }

            // Looping over the whole thing.
            // There should be no New Note commands in the Module anyway.
            m_moduleExt.GetModule().SetRepeatCount(-1);

            // parse info
            m_info = MusicInfoParser.ParseInstrumentInfo(m_moduleExt);

            m_areChannelsSet = false;
            SetNumberVoices(numberVoices);

            ResetVolume();
        }

        #endregion

        #region Impementation of IInstrument

        public string GetLastErrorMessage()
        {
            return m_moduleExt.GetModule().ErrorGetLastMessage();
        }


        public string GetName()
        {
            return m_info.name;
        }

        public string GetComment()
        {
            return m_info.comment;
        }

        public void SetVolume(double volume)
        {
            m_volume = volume;
        }

        public void ResetVolume()
        {
            SetVolume(1.0);
        }

        public void Reset()
        {
            ResetVolume();
        }

        public int PlayNote(MusicalNote note)
        {
            int voice;
            bool shouldPlay = ChooseVoice(note, out voice);
            if (!shouldPlay)
            {
                // couldn't find a proper voice, returning early.
                return -1;
            }

            for (int k = 0; k < GetNumberChannels(); ++k)
            {
                int channel = m_moduleExt.GetInteractive().PlayNote(
                    m_info.instruments[k],
                    note.tone,
                    // volume of the output also depends on the instrument's.
                    m_volume * note.volume,
                    note.panning
                );
                m_playingChannels[voice][k] = channel;
            }
            m_playingNotes[voice] = note;

            return voice;
        }

        /// <summary>
        /// Stops the channels which were playing in voice.
        /// </summary>
        public void StopNote(int voice)
        {
            for (int k = 0; k < GetNumberChannels(); ++k)
            {
                int channel = m_playingChannels[voice][k];
                if (channel != -1)
                {
                    m_moduleExt.GetInteractive().StopNote(channel);
                }
                m_playingChannels[voice][k] = -1;
            }

            // the note is no longer playing.
            m_playingNotes[voice] = new MusicalNote();
        }

        public int GetNumberVoices()
        {
            return m_playingNotes.Length;
        }

        public MusicalNote GetNote(int voice)
        {
            return m_playingNotes[voice];
        }

        public int GetSpeed()
        {
            return m_moduleExt.GetModule().GetCurrentSpeed();
        }

        public int GetCurrentRow()
        {
            return m_moduleExt.GetModule().GetCurrentRow();
        }

        public void SetNumberVoices(int numberVoices)
        {
            if (m_areChannelsSet)
            {
                int previousNumberVoices = GetNumberVoices();
                int[][] playingChannels = new int[numberVoices][];
                MusicalNote[] playingNotes = new MusicalNote[numberVoices];
                for (int voice = 0; voice < System.Math.Max(previousNumberVoices, numberVoices); ++voice)
                {
                    if (voice >= numberVoices)
                    {
                        // if we now have fewer voices to work with,
                        // we should stop the notes we won't be able to remember.
                        StopNote(voice);
                    }
                    else
                    {
                        playingChannels[voice] = new int[GetNumberChannels()];
                        playingChannels[voice].Fill(-1);
                        if (voice >= previousNumberVoices)
                        {
                            playingNotes[voice] = new MusicalNote();
                        }
                        else
                        {
                            playingNotes[voice] = m_playingNotes[voice]; 
                            // otherwise, we pass on the info to the new array.
                            for (int k = 0; k < GetNumberChannels(); ++k)
                            {
                                playingChannels[voice][k] = m_playingChannels[voice][k];
                            }
                        }
                    }
                }
                // finally, replace the arrays.
                m_playingChannels = playingChannels;
                m_playingNotes = playingNotes;
            }
            else
            {
                m_playingNotes = new MusicalNote[numberVoices];
                // fill it with non-playing notes.
                m_playingNotes.Fill(new MusicalNote());
                m_playingChannels = new int[numberVoices][];
                
                for (int voice = 0; voice < GetNumberVoices(); ++voice)
                {
                    m_playingChannels[voice] = new int[GetNumberChannels()];
                    m_playingChannels[voice].Fill(-1);
                }

            }
            m_areChannelsSet = true;
        }

        #endregion

        #region IAudioStream implementation

        public long Read(int sampleRate, long count, float[] mono)
        {
            return m_moduleExt.GetModule().Read(sampleRate, count, mono);
        }

        public long ReadInterleavedQuad(int sampleRate, long count, float[] interleavedQuad)
        {
            return m_moduleExt.GetModule().ReadInterleavedQuad(sampleRate, count, interleavedQuad);
        }

        public long ReadInterleavedStereo(int sampleRate, long count, float[] interleavedStereo)
        {
            return m_moduleExt.GetModule().ReadInterleavedStereo(sampleRate, count, interleavedStereo);
        }

        #endregion

        #region Utility

        /// <summary>
        /// Chooses voice to use to play note, depending on availability.
        /// </summary>
        /// <param name="note"></param>
        /// <param name="chosenVoice"></param>
        /// <returns>Should the note be played?</returns>
        bool ChooseVoice(MusicalNote note, out int chosenVoice)
        {
            if (!note.IsPlaying())
            {
                chosenVoice = -1;
                return false;
            }
            int tone = note.tone;
            // first check for the same tone.
            for (int voice = 0; voice < GetNumberVoices(); ++voice)
            {
                if (GetPlayingTone(voice) == tone)
                {
                    // we do not allow playing twice the same tone on the same instrument.
                    // todo: we could instead replace the note.
                    chosenVoice = voice;
                    return false;
                }
            }
            // else, look for an empty spot.
            for (int voice = 0; voice < GetNumberVoices(); ++voice)
            {
                if (!IsVoicePlaying(voice))
                {
                    chosenVoice = voice;
                    return true;
                }
            }
            // otherwise, failure.
            chosenVoice = -1;
            return false;
        }

        bool IsVoicePlaying(int voice)
        {
            return m_playingNotes[voice].IsPlaying();
        }

        int GetPlayingTone(int voice)
        {
            return m_playingNotes[voice].tone;
        }

        int GetNumberChannels()
        {
            return m_info.instruments.Length;
        }

        #endregion

        #region Private data

        readonly OpenMpt.ModuleExt m_moduleExt;

        readonly InstrumentInfo m_info;

        bool m_areChannelsSet;

        double m_volume;

        int[][] m_playingChannels;
        /// <summary>
        /// call IsPlaying to get the playing state. Call parameterless constructor to create a non-playing note.
        /// </summary>
        MusicalNote[] m_playingNotes;

        #endregion
    }
}