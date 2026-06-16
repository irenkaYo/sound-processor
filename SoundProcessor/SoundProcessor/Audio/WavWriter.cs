using SoundProcessor.Audio.Headers;

namespace SoundProcessor.Audio;

public static class WavWriter
{
    private const uint RIFF = 0x46464952;
    private const uint WAVE = 0x45564157;
    private const uint FMT = 0x20746D66;
    private const uint DATA = 0x61746164;
    
    public static void WriteFile(string fileName, Waveform sound)
    {
        uint dataSize = (uint)(sound.Samples.Count * sizeof(short));

        RiffHeader riff = new RiffHeader
        {
            Sign = RIFF,
            Size = 36 + dataSize,
            WaveId = WAVE
        };

        FmtHeader fmt = new FmtHeader
        {
            ChunkId = FMT,
            ChunkSize = 16,
            FormatTag = 1,
            Channels = sound.Channels,
            SampleRate = sound.SampleRate,
            AvgBytesPerSec = sound.SampleRate * sound.Channels * sound.BitsPerSample / 8,
            BlockAlign = (ushort)(sound.Channels * sound.BitsPerSample / 8),
            BitsPerSample = sound.BitsPerSample
        };

        DataHeader data = new DataHeader
        {
            ChunkId = DATA,
            ChunkSize = dataSize
        };
        
        using BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create));
        
        WriteRiff(writer, riff);
        WriteFmt(writer, fmt);
        WriteData(writer, data);

        foreach (short sample in sound.Samples)
        {
            writer.Write(sample);
        }
    }
    
    private static void WriteRiff(BinaryWriter writer, RiffHeader header)
    {
        writer.Write(header.Sign);
        writer.Write(header.Size);
        writer.Write(header.WaveId);
    }
    
    private static void WriteFmt(BinaryWriter writer, FmtHeader header)
    {
        writer.Write(header.ChunkId);
        writer.Write(header.ChunkSize);
        writer.Write(header.FormatTag);
        writer.Write(header.Channels);
        writer.Write(header.SampleRate);
        writer.Write(header.AvgBytesPerSec);
        writer.Write(header.BlockAlign);
        writer.Write(header.BitsPerSample);
    }
    
    private static void WriteData(BinaryWriter writer, DataHeader header)
    {
        writer.Write(header.ChunkId);
        writer.Write(header.ChunkSize);
    }
}