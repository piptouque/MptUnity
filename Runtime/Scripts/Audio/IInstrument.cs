namespace MptUnity.Audio
{
    /// <summary>
    /// An interface for a the instrument the musician is playing.
    /// </summary>
    public interface IInstrument : IAudioStream
    {
        
        #region Error Handling
        
        string GetLastErrorMessage();
        
        #endregion
        
        #region Playing
        void StopNote(int voice);
        /// <summary>
        /// Plays the MusicalNote on the instrument.
        /// The instrument will keep playing until StopNote is called.
        /// </summary>
        /// <param name="note"></param>
        /// <returns>id of the playing voice, -1 on failure.</returns>
        int PlayNote(MusicalNote note);

        int GetNumberVoices();

        MusicalNote GetNote(int voice);

        /// <summary>
        /// Sets polyphony (maximum number concurrently playing notes.)
        /// </summary>
        /// <param name="numberVoices"></param>
        void SetNumberVoices(int numberVoices);


        /// <summary>
        /// Get playing speed in ticks per row.
        /// </summary>
        /// <returns>speed in ticks per row.</returns>
        int GetSpeed();

        /// <summary>
        /// Get current row in module.
        /// </summary>
        /// <returns></returns>
        int GetCurrentRow();
        #endregion

        #region Settings and info 
        string GetName();
        string GetComment();
        void SetVolume(double volume);
        void ResetVolume();

        /// <summary>
        /// Resets volume and mute state.
        /// </summary>
        void Reset();
        
        #endregion
    }
}