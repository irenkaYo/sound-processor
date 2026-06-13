using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Effects;

public class SilenceFilter : IFilter
{
    private string Unit { get; set; }
    private double Start { get; set; }
    private double End { get; set; }

    public SilenceFilter(string unit, double start, double end)
    {
        Unit = unit;
        Start = start;
        End = end;
    }
    
    public State Apply(Waveform sound)
    {
        double start = Start;
        double end = End;
        
        if (Unit == "ms")
        {
            start /= 1000;
            end /= 1000;
        }
        
        int startSample = (int)Math.Round(start * sound.SampleRate);
        int endSample = (int)Math.Round(end * sound.SampleRate);
        int duration = endSample - startSample;
        
        List<short> zerosList = new List<short>(new short[duration]);
        
        if (startSample <= sound.Samples.Count)
            sound.Samples.InsertRange(startSample, zerosList);
        else
            sound.Samples.AddRange(zerosList);
        
        return State.Ok;
    }
}