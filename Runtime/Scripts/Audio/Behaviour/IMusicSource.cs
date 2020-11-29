namespace MptUnity.Audio.Behaviour
{
    public interface IMusicSource
    {
        
        #region Playback (Unity Audio MonoBehaviour event)
        // Public interface forces this thing to be public
        // But it should be protected in the implementation.
        void OnAudioFilterRead(float[] data, int channels);
        #endregion
    }
}