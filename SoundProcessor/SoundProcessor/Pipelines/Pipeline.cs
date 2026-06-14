using SoundProcessor.Audio;
using SoundProcessor.Filters;

namespace SoundProcessor.Pipelines;

public class Pipeline
{
    private List<IFilter> filters;

    public Pipeline()
    {
        filters = new List<IFilter>();
    }

    public void AddFilter(IFilter filter)
    {
        filters.Add(filter);
    }
    
    public State Execute(Waveform sound)
    {
        foreach (IFilter filter in filters)
        {
            State state = filter.Apply(sound);

            if (state != State.Ok)
                return state;
        }

        return State.Ok;
    }
}