namespace MptUnity.Audio
{
    /// <summary>
    /// A struct for a note played by an Instrument.
    /// Should be one-time use, and constant.
    /// </summary>
    public class MusicalNote
    {
        public readonly int tone;

        public readonly double volume;
        
        public readonly double panning;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_tone">MIDI tone of the note, in semi-tones.</param>
        /// <param name="a_volume">In [0, 1]</param>
        /// <param name="a_panning">In [0, 1]</param>
        public MusicalNote(int a_tone, double a_volume = 1.0, double a_panning = 0.5)
        {
            tone = a_tone;
            volume = a_volume;
            panning = a_panning;
        }

        /// <summary>
        /// Default, a note which is not playing.
        /// </summary>
        public MusicalNote()
        {
            tone = -1;
            volume = 0.0;
            panning = 0.5;
        }

        public bool IsPlaying()
        {
            return tone != -1;
        }

    }
}