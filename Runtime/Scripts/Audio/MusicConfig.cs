namespace MptUnity.Audio
{
    public class MusicConfig
    {
        #region Global config
        
        /// <summary>
        /// Warning: this cannot be called outside the main thread.
        /// </summary>
        /// <returns></returns>
        public static int GetSampleRate()
        {
            return UnityEngine.AudioSettings.GetConfiguration().sampleRate;
        }

        /// <summary>
        /// Warning: this cannot be called outside the main thread.
        /// </summary>
        public static int GetBlockSize()
        {
            return UnityEngine.AudioSettings.GetConfiguration().dspBufferSize;
        }
        
        #endregion

        #region Module rendering

        /// <summary>
        /// Should be in [-1, 10]
        /// -1 means default, 0 means none.
        /// </summary>
        internal const int c_renderVolumeRampingStrength = 10;

        #endregion

        #region Private utility

        void UpdateConfig()
        {
            m_sampleRate = UnityEngine.AudioSettings.GetConfiguration().sampleRate;
            m_blockSize = UnityEngine.AudioSettings.GetConfiguration().dspBufferSize;
        }

        MusicConfig()
        {
           UpdateConfig(); 
        }

        static MusicConfig GetInstance()
        {
            if (s_config == null)
            {
                s_config = new MusicConfig();
            }

            return s_config;
        }

        #endregion

        #region Private static data
        
        static MusicConfig s_config;
        
        int m_sampleRate;
        int m_blockSize;

        #endregion
    }
}