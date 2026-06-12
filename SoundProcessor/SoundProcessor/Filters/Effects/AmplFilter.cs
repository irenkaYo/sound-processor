using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Effects;

public class AmplFilter : IFilter
{
    private double Factor { get; }
    
    public AmplFilter(double factor)
    {
        Factor = factor;
    }
    
    public State Apply(Waveform sound)
    {
        if (sound.Samples == null)
            throw new ArgumentNullException(nameof(sound));
        
        for (int i = 0; i < sound.Samples.Count; i++)
        {
            double operation = Math.Clamp(sound.Samples[i] * Factor, short.MinValue, short.MaxValue);
            sound.Samples[i] = (short)Math.Round(operation);
        }

        return State.Ok;
    }
}