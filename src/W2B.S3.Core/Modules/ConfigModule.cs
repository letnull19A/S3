using System.Text;
using W2B.S3.Core.Configs;
using W2B.S3.Core.Interfaces;
using W2B.S3.Core.Models;

namespace W2B.S3.Core.Modules;

public sealed class ConfigModule(IReadOnlyDictionary<string, string> args) : IControlModule
{
    private string _fileName = string.Empty;
    private string _fileNameExtension = string.Empty;
    private ConfigModel? _configModel;

    public void Init()
    {
    }

    public (string, string, ConfigModel?) Get()
    {
        return (_fileName, _fileNameExtension, _configModel);
    }

    public void Start()
    {
        var isConfigParamExist = IsConfigFileDefined();

        if (isConfigParamExist)
            ConfigsIsDefined();
        else
            ConfigsIsNotDefined();
    }

    public void TakeControl(IControlModule module)
    {
        module.Init();
        module.Start();
    }

    public void End()
    {
        throw new NotImplementedException();
    }

    private bool IsConfigFileDefined()
    {
        return args.ContainsKey("--configFile");
    }

    private void ConfigsIsNotDefined()
    {
        Console.WriteLine("Configuration file is not defined, please set arg --configFile");
    }

    private void ConfigsIsDefined()
    {
        try
        {
            args.TryGetValue("--configFile", out _fileName);

            _fileNameExtension = _fileName.Split('.')[^1];

            var cwd = Directory.GetCurrentDirectory();

            if (!File.Exists(_fileName))
                throw new FileNotFoundException($"""
                                                 failed to load configs:
                                                 file name: {_fileName}
                                                 file ext: {_fileNameExtension}
                                                 CWD: {cwd}
                                                 """);

            var fullPath = Path.GetFullPath(_fileName);

            _configModel = _fileNameExtension switch
            {
                "yaml" => YAMLConfigLoader.LoadConfig(fullPath),
                "json" => JSONConfigLoader.LoadConfig(fullPath),
                _ => _configModel
            };
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}