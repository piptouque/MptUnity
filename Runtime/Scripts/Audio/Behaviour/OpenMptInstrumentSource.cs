
#define FORCE_MONO

namespace MptUnity.Audio.Behaviour
{
    [UnityEngine.RequireComponent(typeof(UnityEngine.AudioSource))]
    public class OpenMptInstrumentSource : AbstractInstrumentSource
    {

        #region AbstractInstrumentSource resolution

        protected override IInstrument CreateInstrument(string path, int a_numberVoices)
        {
            return new OpenMptInstrument(path, a_numberVoices);
        }

        #endregion

        #region Unity MonoBehaviour event

        public override void OnAudioFilterRead(float[] data, int channels)
            {
                if (IsReady())
                {
#if FORCE_MONO
                    // [piptouque]: This seems to work.
                    // It's honestly a proper fix, but what bothers me
                    // is that the issue might come from the C# bindings of OpenMPT,
                    // or me just not RTFM.
                    // todo.
                    long numberFrames = ForceReadMono(data, channels);
#else
                    int sampleRate = m_sampleRate;
                    long numberFrames = 0;
                    switch (channels)
                    {
                        case 1: numberFrames = m_instrument.Read(sampleRate, data.Length, data);
                            break;
                        case 2: numberFrames = m_instrument.ReadInterleavedStereo(sampleRate, data.Length, data);
                            break;
                        case 4: numberFrames = m_instrument.ReadInterleavedQuad(sampleRate, data.Length, data);
                            break;
                        default:
                            UnityEngine.Debug.LogError("Number of channels not supported: " + channels);
                            break;
                    }
#endif
                }
            }

        #endregion

        #region private utility

#if FORCE_MONO
        long ForceReadMono(float[] data, int channels)
        {
            if (m_monoBuffer == null || m_monoBuffer.Length < data.Length / channels)
            {
                m_monoBuffer = new float[data.Length / channels];
            }

            int sampleRate = m_sampleRate;
            long numberFrames = m_instrument.Read(sampleRate, m_monoBuffer.Length, m_monoBuffer);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = m_monoBuffer[i / channels];
            }

            return numberFrames;
        }
#endif
        

        #endregion
        
        #region Private data

        
#if FORCE_MONO
        float[] m_monoBuffer;
#endif

        #endregion

    }
}