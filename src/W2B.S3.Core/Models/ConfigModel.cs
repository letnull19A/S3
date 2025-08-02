namespace W2B.S3.Core.Models;

public sealed class ConfigModel(Network network, RootUser root, ArgsConfig argsConfig)
{
    public ArgsConfig ArgsConfig { get; } = argsConfig;
    public Network Network { get; } = network;
    public RootUser Root { get; } = root;
}

public sealed class ArgsConfig(Network network, RootUser root)
{
    public Network Network { get; } = network;
    public RootUser Root { get; } = root;
}

public sealed class Network(ushort port, string ipAddress)
{
    public ushort Port { get; } = port;
    public string IpAddress { get; } = ipAddress;
}

public sealed class RootUser(string name, string password, string token)
{
    public string? Name { get; } = name;
    public string? Password { get; } = password;
    public string? Token { get; } = token;
}