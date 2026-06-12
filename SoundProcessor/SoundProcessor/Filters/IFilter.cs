using SoundProcessor.Audio;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Filters;

public interface IFilter
{
    public State Apply(Waveform sound);
}