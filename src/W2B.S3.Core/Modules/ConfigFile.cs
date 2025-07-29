using W2B.S3.Core.Interfaces;

namespace W2B.S3.Core.Modules;

public sealed class ConfigFileModule(Dictionary<string, string> args) : IControlModule
{
    private string _fileName = string.Empty;
    private string _fileNameExtension = string.Empty;

    public void Init()
    {
    }

    public (string, string) Get()
    {
        return (_fileName, _fileNameExtension);
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
        args.TryGetValue("--configFile", out _fileName);

        _fileNameExtension = _fileName.Split('.')[^1];
    }
}