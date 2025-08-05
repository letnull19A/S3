using W2B.S3.Core.Interfaces;
using W2B.S3.Core.Models;

namespace W2B.S3.Core.Modules;

public sealed class RootUserModule(IReadOnlyDictionary<string, string> args, ApplicationContextModel applicationContext)
    : IControlModule
{
    private string? _rootUserName = string.Empty;
    private string? _rootUserPassword = string.Empty;
    private string? _rootToken = string.Empty;

    public void Init()
    {
        var isSchemaToken = args.ContainsKey("--rootToken");
        var isSchemaDefault = args.ContainsKey("--rootUser") && args.ContainsKey("--rootPassword");

        if (!(isSchemaToken && isSchemaDefault)) return;

        if (isSchemaDefault && isSchemaToken)
            throw new Exception("Schema authentication conflict with --rootUser and --rootPassword or --rootToken");
        
        if (isSchemaToken)
            RootTokenDefined();
        else
            RootUserDataDefined();
    }

    public void Start()
    {
        if (IsUserDefined()) return;

        var rootUser = applicationContext.Configs.Cluster.Root;

        if (string.IsNullOrEmpty(rootUser.User) && string.IsNullOrEmpty(rootUser.Password) ||
            string.IsNullOrEmpty(rootUser.Token))
        {
        }
    }

    private bool IsUserDefined() => string.IsNullOrEmpty(_rootToken) | string.IsNullOrEmpty(_rootUserName) &&
                                    string.IsNullOrEmpty(_rootUserPassword);

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
    {
    }

    public void End()
    {
        Console.WriteLine("RootUserModule is ended");
    }
}