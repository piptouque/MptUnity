namespace MptUnity.Audio
{
    public class InstrumentInfo
    {
        public readonly string name;
        // id of the instrument(s) in the MOD.
        public readonly int[] instruments;

        public readonly string comment;

        public InstrumentInfo(string a_name, int[] a_instruments, string a_comment)
        {
            name = a_name;
            instruments = (int[]) a_instruments.Clone();
            comment = a_comment;
        }
    }
}