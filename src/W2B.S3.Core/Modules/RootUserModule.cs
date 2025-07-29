using W2B.S3.Core.Interfaces;

namespace W2B.S3.Core.Modules;

public sealed class RootUserModule(IReadOnlyDictionary<string, string> args) : IControlModule
{
    private string? _rootUserName = string.Empty;
    private string? _rootUserPassword = string.Empty;
    private string? _rootToken = string.Empty;

    public void Init()
    { }

    public void Start()
    {
        if (args.ContainsKey("--rootUser") && args.ContainsKey("--rootPassword") && args.ContainsKey("--rootToken"))
            throw new Exception("Can not configure --rootUser and --rootPassword with --rootToken.");

        if (args.ContainsKey("--rootUser") && args.ContainsKey("--rootPassword"))
            RootUserDataDefined();

        if (args.ContainsKey("--rootToken"))
            RootTokenDefined();
    }

    public (string?, string?, string?) Get()
        => (_rootToken, _rootUserName, _rootUserPassword);

    private void RootUserDataDefined()
    {
        args.TryGetValue("--rootUser", out _rootUserName);
        args.TryGetValue("--rootPassword", out _rootUserPassword);

        if (string.IsNullOrEmpty(_rootUserName))
            throw new ArgumentNullException(
                "--rootUser is null or empty, but --rootToken is not defined! Please set value here.");

        if (string.IsNullOrEmpty(_rootUserPassword))
            throw new ArgumentNullException(
                "--rootPassword is null or empty, but --rootToken is not defined! Please set value here.");
    }

    private void RootTokenDefined()
    {
        args.TryGetValue("--rootToken", out _rootToken);

        if (string.IsNullOrEmpty(_rootToken))
            throw new ArgumentNullException(
                "--rootToken is null or empty, but --rootUser and --rootPassword are not defined! Please set value here.");
    }

    public void TakeControl(IControlModule module)
    { }

    public void End()
    {
        Console.WriteLine("RootUserModule is ended");
    }
}