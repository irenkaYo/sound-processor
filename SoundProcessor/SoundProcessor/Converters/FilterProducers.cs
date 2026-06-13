using SoundProcessor.CommandLine;
using SoundProcessor.Filters;
using SoundProcessor.Filters.Effects;

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
}