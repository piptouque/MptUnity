namespace Audio
{
    
    public enum AudioPlaybackState
    {
        ePlaying,
        ePaused,
        eStopped
    }
    
    public interface IPlayable
    {
        

        AudioPlaybackState GetPlaybackState();

        // void SetPlaybackState(AudioPlaybackState state);
        
        // All of the following are just for convenience's sake.
        // Playback can be set from state.
        bool IsPaused();
        bool IsPlaying();
        bool IsStopped();

        void Pause();
        void Play();
        void Stop();

    }
}