namespace W2B.S3.Core.Models;

public sealed class ApplicationContextBuilder
{
    private Dictionary<string, string>? _args;
    private ConfigModel? _configs;

    public void SetArgs(Dictionary<string, string>? args)
    {
        ArgumentNullException.ThrowIfNull(args);
        _args = args;
    }

    public void SetConfigs(ConfigModel? configModel)
    {
        ArgumentNullException.ThrowIfNull(configModel);
        _configs = configModel;
    }

    public ApplicationContextModel Bake()
    {
        if (_args == null || _configs == null) throw new ApplicationException();

        return new ApplicationContextModel(_args, _configs);
    }
}

public sealed class ApplicationContextModel(IReadOnlyDictionary<string, string> args, ConfigModel configs)
{
    public IReadOnlyDictionary<string, string> Args { get; } = args;

    public ConfigModel Configs { get; } = configs;
}