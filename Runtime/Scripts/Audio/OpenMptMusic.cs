using MptUnity.Utility;

namespace MptUnity.Audio
{
    public class OpenMptMusic : IMusic
    {

        #region Life-cycle

        
        public OpenMptMusic(string path) 
        {
            // Load file and get handle to music.
            m_moduleExt =  OpenMptUtility.LoadModuleExt(path);
            if (m_moduleExt == null)
            {
                throw new System.ArgumentException("Error when loading MOD music at path: " + path);
            }
            // parse the info (number of channels etc.) from the message.
            m_info = MusicInfoParser.ParseMusicInfo(m_moduleExt);
            
            // Init sections.
            m_sectionVolumes = new double[m_info.GetNumberSections()];
            m_sectionVolumes.Fill(1.0);

            m_sectionMuted = new bool[m_info.GetNumberSections()];
            m_sectionMuted.Fill(false);
        }
        
        #endregion


        #region IMusic implementation

        public  string GetLastErrorMessage()
        {
            return m_moduleExt.GetModule().ErrorGetLastMessage();
        }

        public  double GetPlayingTempo()
        {
            return m_moduleExt.GetModule().GetCurrentTempo()
                   * m_moduleExt.GetInteractive().GetTempoFactor();
        }
        
        public  double GetTempoFactor()
        {
            return m_moduleExt.GetInteractive().GetTempoFactor();
        }


        public  void SetTempoFactor(double factor)
        {

            // The tempo can be reset at any time by the music.
            // We need to apply an independent modifier.
            m_moduleExt.GetInteractive().SetTempoFactor(factor);
        }


        public int NumberSections => m_info.GetNumberSections();
        public int NumberChannels => m_moduleExt.GetModule().GetNumChannels();

        public int NumberInstruments => m_moduleExt.GetModule().GetNumInstruments();

        public MusicSection GetSection(int sectionIndex)
        {
            return m_info.GetSection(sectionIndex);
        }
        
        public string GetComment()
        {
            return m_info.comment;
        }


        public void SetRepeatCount(int count)
        {
            m_moduleExt.GetModule().SetRepeatCount(count);
        }

        public int GetRepeatCount()
        {
            return m_moduleExt.GetModule().GetRepeatCount();
        }


        public  void MuteSection(int sectionIndex, bool mute)
        {
            foreach (int channel in m_info.GetSectionChannels(sectionIndex))
            {
                m_moduleExt.GetInteractive().SetChannelMuteStatus(channel, mute);
            }
            m_sectionMuted[sectionIndex] = mute;
        }
        
        public void SetSectionVolume(int sectionIndex, double volume)
        {
            foreach (int channel in m_info.GetSectionChannels(sectionIndex))
            {
                m_moduleExt.GetInteractive().SetChannelVolume(channel, volume);
                m_sectionVolumes[sectionIndex] = volume;
            }
        }
        public  void ResetSectionVolume(int sectionIndex)
        {
            SetSectionVolume(sectionIndex, 1.0);
        }

        public  void StopSectionNotes(int sectionIndex)
        {
            foreach (int channel in m_info.GetSectionChannels(sectionIndex))
            {
                m_moduleExt.GetInteractive().StopNote(channel);
            }
        }

        public  bool IsSectionMuted(int sectionIndex)
        {
            return m_sectionMuted[sectionIndex];
        }

        public void ResetSectionMuteStatus(int sectionIndex)
        {
            MuteSection(sectionIndex, false);
        }

        public void Reset()
        {
            m_moduleExt.GetModule().SetPositionOrderRow(0, 0);
            m_moduleExt.GetModule().SetRepeatCount(0);
            
            SetTempoFactor(1D);

            for (int i = 0; i < NumberSections; ++i)
            {
                ResetSectionVolume(i);                
                ResetSectionMuteStatus(i);
            }
        }

        public string GetAuthor()
        {
            return m_info.author;
        }
        
        public string GetTitle()
        {
            return m_info.title;
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

        #region Util
        
        
        #endregion

        #region Internal Data

        readonly OpenMpt.ModuleExt m_moduleExt;
        
        readonly MusicInfo m_info;

        readonly double[] m_sectionVolumes;
        readonly bool[] m_sectionMuted;
        
        #endregion
        
    }
}