using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Effects;

public class TimestretchFilter : IFilter
{
    private double Factor { get; set; }
    
    public TimestretchFilter(double factor)
    {
        Factor = factor;
    }

    public State Apply(Waveform? sound)
    {
        if (sound == null)
            return State.Error;
        
        int newSize = (int)Math.Round(sound.Samples.Count * Factor);
        
        double pos;
        List<short> newSamples = new List<short>(newSize);
        
        for (int i = 0; i < newSize; i++)
        {
            pos = i / Factor;
            int l = (int)Math.Truncate(pos);
            double frac = pos - l;

            short newNumber;
            if (l + 1 < sound.Samples.Count)
                newNumber = (short)(sound.Samples[l] * (1 - frac) + sound.Samples[l + 1] * frac);
            else
                newNumber = sound.Samples[l];
            
            newSamples.Add(newNumber);
        }
        sound.Samples = newSamples;
        return State.Ok;
    }
}