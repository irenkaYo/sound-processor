using SoundProcessor.CommandLine;
using SoundProcessor.Filters;
using SoundProcessor.Pipelines;

namespace SoundProcessor.Converters;

public class CmdLineArgs2PipelineConverter
{
    public delegate IFilter FilterProducer(FilterDescriptor descriptor);
    private readonly Dictionary<string, FilterProducer> _producers;

    public CmdLineArgs2PipelineConverter()
    {
        _producers = new Dictionary<string, FilterProducer>();
    }

    public Pipeline CreatePipeline(List<FilterDescriptor> descriptors)
    {
        Pipeline pipeline = new Pipeline();
        foreach (var descriptor in descriptors)
        {
            _producers.TryGetValue(descriptor.Name, out FilterProducer producer);
            
            if (producer == null)
                throw new InvalidDataException($"Filter {descriptor.Name} does not exist");
            
            IFilter filter = producer(descriptor);
            pipeline.AddFilter(filter);
        }
        return pipeline;
    }
    
    public void AddFilterProducer(string filterName, FilterProducer producer)
    {
        _producers[filterName] = producer;
    }
}