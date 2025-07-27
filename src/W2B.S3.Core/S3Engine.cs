using Microsoft.Extensions.Logging;
using W2B.S3.Core.Interfaces;
using W2B.S3.Core.Modules;
using W2B.S3.Core.Utils;

namespace W2B.S3.Core;

public sealed class S3Engine : IControlModule<S3Engine, string[]>
{
    private Dictionary<string, string> _parsedArgs;
    private string _fileName = string.Empty;
    private string _fileNameExtension = string.Empty;

    public S3Engine Init(string[] args)
    {
        var parser = new ArgsParser(args);
        _parsedArgs = parser.Parse();

        return this;
    }

    public void Start()
    {
        var configs = new ConfigFile();
        
        configs.Init(_parsedArgs)
            .Start();
        
        DisplayFinallyConfigs();
    }

    public void TakeControl(IControlModule<S3Engine, string[]> module)
    { }

    public void End()
    {
        Console.WriteLine("S3Engine is end execute instructions");
    }

    private void DisplayFinallyConfigs()
    {
        Console.WriteLine("""

                          ============ Configs ============
                          """);

        Console.Write("=> loaded schema: ");

        switch (_fileNameExtension)
        {
            case "yaml":
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case "json":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "env":
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
        }

        Console.WriteLine(_fileNameExtension.ToUpper());
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine($"=> schema name: {_fileName}");
    }
}