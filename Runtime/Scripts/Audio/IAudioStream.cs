namespace MptUnity.Audio
{
    public interface IAudioStream
    {

        long Read(int sampleRate, long count, float[] mono);
        long ReadInterleavedQuad(int sampleRate, long count, float[] interleavedQuad);
        long ReadInterleavedStereo(int sampleRate, long count, float[] interleavedStereo);
    }
}