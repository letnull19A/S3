using W2B.S3.Core.Interfaces;
using W2B.S3.Core.Models;
using W2B.S3.Core.Modules;
using W2B.S3.Core.Utils;

namespace W2B.S3.Core;

public sealed class S3Engine(IReadOnlyList<string> args) : IControlModule
{
    private Dictionary<string, string> _parsedArgs = new();

    private string _fileName = string.Empty;
    private string _fileNameExtension = string.Empty;

    private ConfigModel? _config;
    
    public void Init()
    {
        var parser = new ArgsParser(args);
        _parsedArgs = parser.Parse();
    }

    public void Start()
    {
        var configs = new ConfigModule(_parsedArgs);

        configs.Init();
        configs.Start();

        (_fileName, _fileNameExtension, _config) = configs.Get();

        // Check db connection

        var rootUser = new RootUserModule(_parsedArgs);

        rootUser.Init();
        rootUser.Start();

        DisplayFinallyConfigs();
    }

    public void TakeControl(IControlModule module)
    {
    }

    public void End()
    {
    }

    private string GetOperationSystemName() => OperatingSystem.IsAndroid() ? "Android" :
        OperatingSystem.IsLinux() ? "Linux" :
        OperatingSystem.IsWindows() ? "Windows" : "Not defined";

    private void DisplayFinallyConfigs()
    {
        Console.WriteLine("""

                          ============ Configs ============
                          """);

        Console.Write("=> loaded schema: ");

        Console.ForegroundColor = _fileNameExtension switch
        {
            "yaml" => ConsoleColor.Green,
            "json" => ConsoleColor.Yellow,
            "env" => ConsoleColor.Magenta,
            _ => Console.ForegroundColor
        };

        Console.WriteLine(_fileNameExtension.ToUpper());
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine($"=> schema name: {_fileName}");

        var schema = string.IsNullOrEmpty(_config?.Cluster.Root.Token) ? "login, password" : "token";

        Console.Write($"=> authentication root user schema: ");

        Console.ForegroundColor = schema switch
        {
            "token" => ConsoleColor.Cyan,
            "login, password" => ConsoleColor.Yellow,
            _ => Console.ForegroundColor
        };

        Console.WriteLine(schema);

        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine(schema != "token"
            ? $"""
               => root user: {_config?.Cluster.Root.User}
               => root password: ***
               """
            : $"=> root token: {_config?.Cluster.Root.Token[..4]}*********\n" +
              $"=> operating system: {GetOperationSystemName()}");
    }
}
