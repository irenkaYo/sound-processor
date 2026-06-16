using SoundProcessor;
using SoundProcessor.Converters;

class Program
{
    static void Main(string[] args)
    {
        CmdLineArgs2PipelineConverter converter = new CmdLineArgs2PipelineConverter();
        Application application = new Application(converter);
        application.Run(args);
    }
}