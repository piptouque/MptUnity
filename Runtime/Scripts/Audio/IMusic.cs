namespace MptUnity.Audio
{
        
    
    public interface IMusic : IAudioStream
    {

        #region Error Handling
        string GetLastErrorMessage();
        
        #endregion
        
        #region Settings and info 

        double GetPlayingTempo();
        double GetTempoFactor();
        void SetTempoFactor(double factor);
        
        int NumberSections { get; }
        
        int NumberChannels { get;  }
        int NumberInstruments { get;  }
        
        string GetAuthor();

        string GetTitle();
        string GetComment();


        MusicSection GetSection(int sectionIndex);


        /// <summary>
        /// Sets the music repeat state.
        ///   -1 loops indefinitely.
        ///   0 plays once.
        ///   n > 0 plays once then repeats n times.
        /// </summary>
        /// <param name="count"></param>
        void SetRepeatCount(int count);
        int GetRepeatCount();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionIndex"></param>
        /// <param name="volume">In [0, 1]</param>
        void SetSectionVolume(int sectionIndex, double volume);
        void ResetSectionVolume(int sectionIndex);

        /// <summary>
        /// Stop all currently playing notes in the section at sectionIndex.
        /// </summary>
        /// <param name="sectionIndex"></param>
        void StopSectionNotes(int sectionIndex);
        /// <summary>
        /// Sets volume the section to 0 or previously set volume.
        /// </summary>
        /// <param name="sectionIndex"></param>
        /// <param name="mute">if true, then set to 0, else set to previous volume.</param>
        void MuteSection(int sectionIndex, bool mute);

        bool IsSectionMuted(int sectionIndex);
        void ResetSectionMuteStatus(int sectionIndex);

        /// <summary>
        /// Resets BPM, volumes, repeat and mute state and rewinds the music.
        /// </summary>
        void Reset();
        
        #endregion



    }
}