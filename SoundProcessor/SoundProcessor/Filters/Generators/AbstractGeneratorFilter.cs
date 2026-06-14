using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters.Generators;

public abstract class AbstractGeneratorFilter : IFilter
{
    public abstract State Apply(Waveform sound);
}