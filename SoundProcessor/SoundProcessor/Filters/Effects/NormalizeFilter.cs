using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Effects;

public class NormalizeFilter : IFilter
{
    private double Peak { get; set; }

    public NormalizeFilter(double peak)
    {
        Peak = peak;
    }
    
    public NormalizeFilter()
    {
        Peak = 1;
    }

    public State Apply(Waveform? sound)
    {
        if (sound == null)
            return State.Error;
        
        int currentPeak = sound.Samples
            .Select(s => Math.Abs((int)s))
            .Max();

        if (currentPeak == 0)
            return State.Ok;
        
        double scale = Peak * short.MaxValue / currentPeak;

        for (int i = 0; i < sound.Samples.Count; i++)
        {
            double operation = Math.Clamp(sound.Samples[i] * scale, short.MinValue, short.MaxValue);
            sound.Samples[i] = (short)Math.Round(operation);
        }
        
        return State.Ok;
    }
}