using SoundProcessor;
using SoundProcessor.Converters;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            CmdLineArgs2PipelineConverter converter = new CmdLineArgs2PipelineConverter();
            Application application = new Application(converter);
            application.Run(args);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
            Environment.Exit(1);
        }
    }
}