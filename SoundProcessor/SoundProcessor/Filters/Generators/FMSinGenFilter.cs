using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Generators;

public class FMSinGenFilter : AbstractGeneratorFilter
{
    private double Amplitude { get;  set; }
    private double CarrierHz { get;  set; }
    private double ModulationHz { get;  set; }
    private double DeviationHz { get;  set; }
    private double DurationMs { get;  set; }

    public FMSinGenFilter(double amplitude, double carrierHz, double modulationHz, double deviationHz, double durationMs)
    {
        Amplitude = amplitude;
        CarrierHz = carrierHz;
        ModulationHz = modulationHz;
        DeviationHz = deviationHz;
        DurationMs = durationMs;
    }

    public override State Apply(Waveform? sound)
    {
        if (sound == null)
            return State.Error;
        
        int duration = (int)Math.Round(DurationMs * sound.SampleRate / 1000);

        for (int i = 0; i < duration; i++)
        {
            double t = (double)i / sound.SampleRate;

            double phase = 2 * Math.PI * CarrierHz * t + (DeviationHz / ModulationHz) * Math.Sin(2 * Math.PI * ModulationHz * t);
            short result = (short)Math.Clamp(Math.Round(Amplitude * short.MaxValue * Math.Sin(phase)), short.MinValue, short.MaxValue);
            sound.Samples.Add(result);
        }

        return State.Ok;
    }
}