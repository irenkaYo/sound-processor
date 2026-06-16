namespace SoundProcessor.Audio.Headers;

public class FmtHeader
{
    public uint ChunkId;
    public uint ChunkSize;
    public ushort FormatTag;
    public ushort Channels;
    public uint SampleRate;
    public uint AvgBytesPerSec;
    public ushort BlockAlign;
    public ushort BitsPerSample;
}