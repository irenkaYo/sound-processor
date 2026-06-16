namespace SoundProcessor.Audio;

public class Waveform
{
    public ushort Channels { get; set; }
    public uint SampleRate { get; set; }
    public ushort BitsPerSample { get; set; }
    public List<short> Samples { get; set; }

    public Waveform(ushort channels, uint sampleRate, ushort bitsPerSample, List<short> samples)
    {
        Channels = channels;
        SampleRate = sampleRate;
        BitsPerSample = bitsPerSample;
        Samples = samples;
    }

    public Waveform()
    {
        Channels = 1;
        SampleRate = 44100;
        BitsPerSample = 16;
        Samples = new List<short>();
    }
}