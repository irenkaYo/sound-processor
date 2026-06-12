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
        return new LowpassFilter(windowSize);
    }
    
    public static IFilter NormalizeFilterCreator(FilterDescriptor descriptor)
    {
        double peak = double.Parse(descriptor.Parameters[0]);
        return new NormalizeFilter(peak);
    }
    
    public static IFilter SilenceFilterCreator(FilterDescriptor descriptor)
    {
        string unit = descriptor.Parameters[0];
        double start = double.Parse(descriptor.Parameters[1]);
        double end = double.Parse(descriptor.Parameters[2]);
        return new SilenceFilter(unit, start, end);
    }

    public static IFilter TimestretchFilterCreator(FilterDescriptor descriptor)
    {
        double factor = double.Parse(descriptor.Parameters[0]);
        return new TimestretchFilter(factor);
    }
}