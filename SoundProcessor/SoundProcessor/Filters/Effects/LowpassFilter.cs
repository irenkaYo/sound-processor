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

    public State Apply(Waveform sound)
    {
        List<short> newSamples = new List<short>();

        int sum = 0;

        int elementsCount = WindowSize / 2;
        for (int i = 0; i < sound.Samples.Count; i++)
        {
            if (i == 0)
            {
                int k = elementsCount;
                while (k != 0)
                {
                    sum += sound.Samples[k];
                    k--;
                }
                sum += sound.Samples[i] * (elementsCount + 1);
            }
            else if (i - elementsCount - 1 < 0)
            {
                sum = sum - sound.Samples[0] + sound.Samples[i + elementsCount];
            }
            else if (i + elementsCount >= sound.Samples.Count)
            {
                sum = sum - sound.Samples[i - elementsCount - 1] + sound.Samples[sound.Samples.Count - 1];
            }
            else
            {
                sum = sum - sound.Samples[i - elementsCount - 1] + sound.Samples[i + elementsCount];
            }
            
            short avg = (short)(sum / WindowSize);
            newSamples.Add(avg);
        }

        return State.Ok;
    }
}