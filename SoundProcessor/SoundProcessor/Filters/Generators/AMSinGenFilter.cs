using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Generators;

public class AMSinGenFilter : AbstractGeneratorFilter
{
    private double Amplitude { get;  set; }
    private double CarrierHz { get;  set; }
    private double ModulationHz { get;  set; }
    private double Depth { get;  set; }
    private double DurationMs { get;  set; }

    public AMSinGenFilter(double amplitude, double carrierHz, double modulationHz, double depth, double durationMs)
    {
        Amplitude = amplitude;
        CarrierHz = carrierHz;
        ModulationHz = modulationHz;
        Depth = depth;
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

            double envelope = 1 + Depth * Math.Sin(2 * Math.PI * ModulationHz * t);
            double carrier = Math.Sin(2 * Math.PI * CarrierHz * t);
            short result = (short)Math.Clamp(Math.Round(Amplitude * 32767 * envelope * carrier), short.MinValue, short.MaxValue);
            sound.Samples.Add(result);
        }

        return State.Ok;
    }
}