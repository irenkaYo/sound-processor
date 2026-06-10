namespace SoundProcessor.CommandLine;

public class ArgsParser
{
    public string? InputFileName { get; set; }
    public string? OutputFileName { get; set; }
    public string[] Args { get; }
    public List<FilterDescriptor> Filters { get; }

    public ArgsParser(string[] args)
    {
        Args = args;
        Filters = new List<FilterDescriptor>();
    }

    public ParseResult ParseArguments()
    {
        if (Args.Length == 0)
            return ParseResult.NoArgs;

        int i = 0;
        while (i < Args.Length)
        {
            if (Args[i] == "-i")
            {
                if (i + 1 == Args.Length || IsFlag(Args[i + 1]) || InputFileName != null)
                    return ParseResult.BadArgs;
                InputFileName = Args[i + 1];
                i += 2;
            }
            else if (Args[i] == "-o")
            {
                if (i + 1 == Args.Length || IsFlag(Args[i + 1]) || OutputFileName != null)
                    return ParseResult.BadArgs;
                OutputFileName = Args[i + 1];
                i += 2;
            }
            else if (Args[i] == "-f")
            {
                if (i + 1 == Args.Length || IsFlag(Args[i + 1]))
                    return ParseResult.BadArgs;
                
                string filterName = Args[i + 1];
                
                List<string> parameters = new List<string>();
                i += 2;
                while (i < Args.Length && !IsFlag(Args[i]))
                {
                    parameters.Add(Args[i]);
                    i++;
                }
                    
                FilterDescriptor descriptor = new FilterDescriptor(filterName, parameters);
                Filters.Add(descriptor);
            }
            else 
                return ParseResult.BadArgs;
        }
        
        if (InputFileName == null && OutputFileName == null ||
            InputFileName != null && OutputFileName == null)
            return ParseResult.BadArgs;
        
        return ParseResult.Ok;
    }

    private bool IsFlag(string arg)
    {
        return arg == "-f" || arg == "-o" || arg == "-i";
    }
}