namespace MptUnity.Audio
{
    /// <summary>
    /// Parts are neither MOD Channels nor instruments.
    /// Rather, they are the list of channels that is reserved for each instruments.
    /// </summary>
    public class MusicSection
    {
        // Name of the associated instrument.
        public readonly string name;
        // Channels owned by the instrument.
        public readonly int[] channels;

        public MusicSection(string a_name, int[] a_channels)
        {
            name = a_name;
            channels = (int[]) a_channels.Clone();
        }

    }
}