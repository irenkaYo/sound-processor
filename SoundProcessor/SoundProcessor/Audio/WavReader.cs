using SoundProcessor.Audio.Headers;

namespace SoundProcessor.Audio;

public static class WavReader
{
    private const uint WAVE = 0x45564157;
    private const uint RIFF = 0x46464952;
    private const uint FMT  = 0x20746D66;
    private const uint DATA = 0x61746164;
    
    public static Waveform ReadFile(string fileName)
    {
        using BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open));
        
        RiffHeader riff = new RiffHeader
        {
            Sign = reader.ReadUInt32(),
            Size = reader.ReadUInt32(),
            WaveId = reader.ReadUInt32()
        };
        
        if (riff.Sign != RIFF)
            throw new InvalidDataException("File is not a RIFF file");
        
        if (riff.WaveId != WAVE)
        {
            throw new InvalidDataException("File is not WAVE");
        }
        
        FmtHeader fmt = new FmtHeader
        {
            ChunkId = reader.ReadUInt32(),
            ChunkSize = reader.ReadUInt32(),
            FormatTag = reader.ReadUInt16(),
            Channels = reader.ReadUInt16(),
            SampleRate = reader.ReadUInt32(),
            AvgBytesPerSec = reader.ReadUInt32(),
            BlockAlign = reader.ReadUInt16(),
            BitsPerSample = reader.ReadUInt16()
        };
        
        if (fmt.ChunkId != FMT)
            throw new InvalidDataException("Expected 'fmt ' chunk");
        
        DataHeader data = new DataHeader
        {
            ChunkId = reader.ReadUInt32(),
            ChunkSize = reader.ReadUInt32()
        };
        
        if (data.ChunkId != DATA)
            throw new InvalidDataException("Expected 'data' chunk");
        
        int samplesCount = (int)(data.ChunkSize / 2);

        List<short> samples = new List<short>(samplesCount);

        for (int i = 0; i < samplesCount; i++)
        {
            samples.Add(reader.ReadInt16());
        }

        Waveform waveform = new Waveform(fmt.Channels, fmt.SampleRate, fmt.BitsPerSample, samples);
        
        return waveform;
    }
}