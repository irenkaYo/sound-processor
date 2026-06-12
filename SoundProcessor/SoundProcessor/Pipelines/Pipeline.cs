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
}