using W2B.S3.Core.Interfaces;

namespace W2B.S3.Core.Modules;

public sealed class ConfigFile : IControlModule<ConfigFile, Dictionary<string, string>>
{
    private Dictionary<string, string> _parsedArgs = new();
    private string _fileName = string.Empty;
    private string _fileNameExtension = string.Empty;
    
    public ConfigFile Init(Dictionary<string, string> args)
    {
        _parsedArgs = args;

        return this;
    }

    public void Start()
    {
        var isConfigParamExist = IsConfigFileDefined();

        if (isConfigParamExist)
            ConfigsIsDefined();
        else
            ConfigsIsNotDefined();
    }

    public void TakeControl(IControlModule<ConfigFile, Dictionary<string, string>> module)
    {
        throw new NotImplementedException();
    }

    public void End()
    {
        throw new NotImplementedException();
    }

    private bool IsConfigFileDefined()
    {
        return _parsedArgs.ContainsKey("--configFile");
    }

    private void ConfigsIsNotDefined()
    {
        Console.WriteLine("Configuration file is not defined, please set arg --configFile");
    }

    private void ConfigsIsDefined()
    {
        _parsedArgs.TryGetValue("--configFile", out _fileName);

        _fileNameExtension = _fileName.Split('.')[^1];
    }
}