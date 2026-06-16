using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Generators;

public class SinGenFilter : AbstractGeneratorFilter
{
    private double FrequencyHz { get; set; }
    private double DurationMs { get; set; }

    public SinGenFilter(double frequencyHz, double durationMs)
    {
        FrequencyHz = frequencyHz;
        DurationMs = durationMs;
    }
    
    public override State Apply(Waveform? sound)
    {
        if (sound == null)
            return State.Error;
        
        sound.Samples.Clear();
        
        int duration = (int)Math.Round(DurationMs * sound.SampleRate / 1000);
        
        for (int i = 0; i < duration; i++)
        {
            double t = (double)i / sound.SampleRate;
            
            sound.Samples.Add((short)Math.Round(short.MaxValue * Math.Sin(2 * Math.PI * FrequencyHz * t)));
        }

        return State.Ok;
    }
}