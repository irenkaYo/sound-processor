using SoundProcessor.Audio;
using SoundProcessor.CommandLine;
using SoundProcessor.Converters;
using SoundProcessor.Pipelines;

namespace SoundProcessor;

public class Application
{
    private CmdLineArgs2PipelineConverter _cmdConverter;

    public Application(CmdLineArgs2PipelineConverter cmdConverter)
    {
        _cmdConverter = cmdConverter;
    }

    private void Configure()
    {
        _cmdConverter.AddFilterProducer("ampl", FilterProducers.AmplFilterCreator);
        _cmdConverter.AddFilterProducer("normalize", FilterProducers.NormalizeFilterCreator);
        _cmdConverter.AddFilterProducer("silence", FilterProducers.SilenceFilterCreator);
        _cmdConverter.AddFilterProducer("timestretch", FilterProducers.TimestretchFilterCreator);
        _cmdConverter.AddFilterProducer("lowpass", FilterProducers.LowpassFilterCreator);
        _cmdConverter.AddFilterProducer("generator", FilterProducers.GeneratorFilterCreator);
    }

    public void Run(string[] args)
    {
        Configure();
        ArgsParser parser = new ArgsParser(args);

        ParseResult parseResult = parser.ParseArguments();
        
        if (parseResult == ParseResult.NoArgs)
        {
            Console.WriteLine("Usage: SoundProcessor [-i input.wav] -o output.wav [-f filter params...]");
            return;
        }

        if (parseResult != ParseResult.Ok)
            throw new Exception("Argument error: " + parseResult);
        
        Pipeline pipeline = _cmdConverter.CreatePipeline(parser.Filters);

        Waveform sound;
        if (parser.InputFileName != null)
            sound = WavReader.ReadFile(parser.InputFileName);
        else
            sound = new Waveform();
        
        State state = pipeline.Execute(sound);
        
        if (state != State.Ok)
        {
            throw new Exception($"Pipeline failed: {state}");
        }
        
        if (parser.OutputFileName != null)
            WavWriter.WriteFile(parser.OutputFileName, sound);
    }
}