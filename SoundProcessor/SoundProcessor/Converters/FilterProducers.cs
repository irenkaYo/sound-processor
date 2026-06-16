using System.Globalization;
using SoundProcessor.CommandLine;
using SoundProcessor.Filters;
using SoundProcessor.Filters.Effects;
using SoundProcessor.Filters.Generators;

namespace SoundProcessor.Converters;

public static class FilterProducers
{
    public static IFilter AmplFilterCreator(FilterDescriptor descriptor)
    {
        if (descriptor.Parameters.Count != 1)
            throw new ArgumentException("Ampl filter requires 1 parameter");
        
        double factor = ParseDouble(descriptor.Parameters[0], nameof(factor));
        
        if (factor < 0)
            throw new ArgumentException("Ampl filter cannot be negative");
        
        return new AmplFilter(factor);
    }
    
    public static IFilter LowpassFilterCreator(FilterDescriptor descriptor)
    {
        if (descriptor.Parameters.Count != 1)
            throw new ArgumentException("Lowpass filter requires 1 parameter");

        if (!int.TryParse(descriptor.Parameters[0], out int windowSize))
            throw new ArgumentException("Window size must be an integer");
        
        if (windowSize < 1 || windowSize % 2 == 0)
            throw new ArgumentException("Lowpass filter wrong input");
        
        return new LowpassFilter(windowSize);
    }
    
    public static IFilter NormalizeFilterCreator(FilterDescriptor descriptor)
    {
        if (descriptor.Parameters.Count > 1)
            throw new ArgumentException("Normalize filter accepts 0 or 1 parameter");

        if (descriptor.Parameters.Count == 0)
            return new NormalizeFilter();

        double peak = ParseDouble(descriptor.Parameters[0], nameof(peak));

        if (peak < 0 || peak > 1)
            throw new ArgumentException("Peak must be in range [0, 1]");

        return new NormalizeFilter(peak);
    }
    
    public static IFilter SilenceFilterCreator(FilterDescriptor descriptor)
    {
        if (descriptor.Parameters.Count != 3)
            throw new ArgumentException("Silence filter requires 3 parameters");

        string unit = descriptor.Parameters[0];

        if (unit != "ms" && unit != "sec")
            throw new ArgumentException("Unknown unit");

        double start = ParseDouble(descriptor.Parameters[1],nameof(start));

        double end =  ParseDouble(descriptor.Parameters[2], nameof(end));

        if (start < 0 || end < start)
            throw new ArgumentException("Invalid range");

        return new SilenceFilter(unit, start, end);
    }

    public static IFilter TimestretchFilterCreator(FilterDescriptor descriptor)
    {
        if (descriptor.Parameters.Count != 1)
            throw new ArgumentException("Timestretch filter requires 1 parameter");

        double factor = ParseDouble(descriptor.Parameters[0], nameof(factor));
        
        if (factor <= 0)
            throw new ArgumentException("Timestretch filter cannot be negative");
        
        return new TimestretchFilter(factor);
    }

    public static IFilter GeneratorFilterCreator(FilterDescriptor descriptor)
    {
        if (descriptor.Parameters.Count < 1)
            throw new ArgumentException("Generator filter requires a type (sin, am, fm)");
        
        string type = descriptor.Parameters[0];
        
        IFilter generator;
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
                throw new ArgumentException("Filter generator type not supported");
            }
        }

        return generator;
    }
    
    private static IFilter SinGenFilterCreator(FilterDescriptor descriptor)
    {
        double frequencyHz = ParseDouble(descriptor.Parameters[1], nameof(frequencyHz));
        double durationMs = ParseDouble(descriptor.Parameters[2], nameof(durationMs));
        
        if (frequencyHz < 0 || durationMs < 0)
            throw new ArgumentException("SinGen filter cannot be negative");
        
        return new SinGenFilter(frequencyHz, durationMs);
    }

    private static IFilter AMSinGenFilterCreator(FilterDescriptor descriptor)
    {
        double amplitude = ParseDouble(descriptor.Parameters[1], nameof(amplitude));
        double carrierHz = ParseDouble(descriptor.Parameters[2], nameof(carrierHz));
        double modulationHz = ParseDouble(descriptor.Parameters[3], nameof(modulationHz));
        double depth = ParseDouble(descriptor.Parameters[4], nameof(depth));
        double durationMs = ParseDouble(descriptor.Parameters[5], nameof(durationMs));
        
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
        double amplitude = ParseDouble(descriptor.Parameters[1], nameof(amplitude));
        double carrierHz = ParseDouble(descriptor.Parameters[2], nameof(carrierHz));
        double modulationHz = ParseDouble(descriptor.Parameters[3], nameof(modulationHz));
        double deviationHz = ParseDouble(descriptor.Parameters[4], nameof(deviationHz));
        double durationMs = ParseDouble(descriptor.Parameters[5], nameof(durationMs));
        
        if (amplitude < 0 || amplitude > 1 ||
            carrierHz < 0||
            modulationHz <= 0 ||
            deviationHz < 0 ||
            durationMs < 0)
            throw new ArgumentException("FM SinGen filter wrong input");
        
        return new FMSinGenFilter(amplitude, carrierHz, modulationHz, deviationHz, durationMs);
    }
    
    private static double ParseDouble(string value, string name)
    {
        if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            throw new ArgumentException($"{name} must be a number");

        return result;
    }
}