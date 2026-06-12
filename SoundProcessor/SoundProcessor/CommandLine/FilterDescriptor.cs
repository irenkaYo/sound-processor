namespace SoundProcessor.CommandLine;

public class FilterDescriptor
{
    public string Name { get; set; }
    public List<string> Parameters { get; set; }

    public FilterDescriptor(string name, List<string> parameters)
    {
        Name = name;
        Parameters = parameters;
    }
}