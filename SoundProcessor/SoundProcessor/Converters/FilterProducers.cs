using SoundProcessor.CommandLine;
using SoundProcessor.Filters;
using SoundProcessor.Filters.Effects;
using SoundProcessor.Filters.Generators;

namespace SoundProcessor.Converters;

public static class FilterProducers
{
    public static IFilter AmplFilterCreator(FilterDescriptor descriptor)
    {
        double factor = double.Parse(descriptor.Parameters[0]);
        
        if (factor < 0)
            throw new ArgumentException("Ampl filter cannot be negative");
        
        return new AmplFilter(factor);
    }
    
    public static IFilter LowpassFilterCreator(FilterDescriptor descriptor)
    {
        int windowSize = int.Parse(descriptor.Parameters[0]);
        
        if (windowSize < 1 || windowSize % 2 == 0)
            throw new ArgumentException("Lowpass filter wrong input");
        
        return new LowpassFilter(windowSize);
    }
    
    public static IFilter NormalizeFilterCreator(FilterDescriptor descriptor)
    {
        double peak;
        if (descriptor.Parameters.Count > 0)
            peak = double.Parse(descriptor.Parameters[0]);
        else
            return new NormalizeFilter();
        
        if (peak < 0 || peak > 1)
            throw new ArgumentException("Normalize filter must be from 0 to 1");
        
        return new NormalizeFilter(peak);
    }
    
    public static IFilter SilenceFilterCreator(FilterDescriptor descriptor)
    {
        string unit = descriptor.Parameters[0];
        double start = double.Parse(descriptor.Parameters[1]);
        double end = double.Parse(descriptor.Parameters[2]);
        
        if (start < 0 || end < start)
            throw new ArgumentException("Silence filter wrong input");
        
        return new SilenceFilter(unit, start, end);
    }

    public static IFilter TimestretchFilterCreator(FilterDescriptor descriptor)
    {
        double factor = double.Parse(descriptor.Parameters[0]);
        
        if (factor < 0)
            throw new ArgumentException("Timestretch filter cannot be negative");
        
        return new TimestretchFilter(factor);
    }

    public static IFilter GeneratorFilterCreator(FilterDescriptor descriptor)
    {
        string type = descriptor.Parameters[0];
        
        IFilter generator = null;
        switch (type)
        {
            case "sin":
            {
                generator = SinGenFilterCreator(descriptor);
                break;
            }
            case "am":
            {
                generator = AMSinGenFilterCreator(descriptor);
                break;
            }
            case "fm":
            {
                generator = FMSinGenFilterCreator(descriptor);
                break;
            }
            default:
            {
                throw new ArgumentException("Filter type not supported");
            }
        }

        return generator;
    }
    
    private static IFilter SinGenFilterCreator(FilterDescriptor descriptor)
    {
        double frequencyHz = double.Parse(descriptor.Parameters[1]);
        double durationMs = double.Parse(descriptor.Parameters[2]);
        
        if (frequencyHz < 0 || durationMs < 0)
            throw new ArgumentException("SinGen filter cannot be negative");
        
        return new SinGenFilter(frequencyHz, durationMs);
    }

    private static IFilter AMSinGenFilterCreator(FilterDescriptor descriptor)
    {
        double amplitude = double.Parse(descriptor.Parameters[1]);
        double carrierHz = double.Parse(descriptor.Parameters[2]);
        double modulationHz = double.Parse(descriptor.Parameters[3]);
        double depth = double.Parse(descriptor.Parameters[4]);
        double durationMs = double.Parse(descriptor.Parameters[5]);
        
        if (amplitude < 0 || amplitude > 1 ||
            carrierHz < 0||
            modulationHz < 0 ||
            depth < 0 || depth > 1 ||
            durationMs < 0)
            throw new ArgumentException("AM SinGen filter wrong input");
        
        return new AMSinGenFilter(amplitude, carrierHz, modulationHz, depth, durationMs);
    }

    private static IFilter FMSinGenFilterCreator(FilterDescriptor descriptor)
    {
        double amplitude = double.Parse(descriptor.Parameters[1]);
        double carrierHz = double.Parse(descriptor.Parameters[2]);
        double modulationHz = double.Parse(descriptor.Parameters[3]);
        double deviationHz = double.Parse(descriptor.Parameters[4]);
        double durationMs = double.Parse(descriptor.Parameters[5]);
        
        if (amplitude < 0 || amplitude > 1 ||
            carrierHz < 0||
            modulationHz <= 0 ||
            deviationHz < 0 ||
            durationMs < 0)
            throw new ArgumentException("FM SinGen filter wrong input");
        
        return new FMSinGenFilter(amplitude, carrierHz, modulationHz, deviationHz, durationMs);
    }
}