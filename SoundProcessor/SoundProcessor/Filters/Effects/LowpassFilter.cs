using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Effects;

public class LowpassFilter : IFilter
{
    private int WindowSize { get; set; }

    public LowpassFilter(int windowSize)
    {
        WindowSize = windowSize;
    }

    public State Apply(Waveform? sound)
    {
        if (sound == null)
            return State.Error;

        if (sound.Samples.Count == 0)
            return State.Ok;

        List<short> newSamples = new List<short>(sound.Samples.Count);

        int halfWindow = WindowSize / 2;

        long sum = 0;
        
        for (int j = -halfWindow; j <= halfWindow; j++)
        {
            sum += GetSample(sound.Samples, j);
        }

        for (int i = 0; i < sound.Samples.Count; i++)
        {
            newSamples.Add((short)(sum / WindowSize));

            if (i == sound.Samples.Count - 1)
                break;

            sum -= GetSample(sound.Samples, i - halfWindow);
            sum += GetSample(sound.Samples, i + halfWindow + 1);
        }

        sound.Samples = newSamples;

        return State.Ok;
    }
    
    private short GetSample(List<short> samples, int index)
    {
        if (index < 0)
            return samples[0];

        if (index >= samples.Count)
            return samples[^1];

        return samples[index];
    }
}