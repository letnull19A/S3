namespace W2B.S3.Core.Models;

public sealed class ConfigModel
{
    public Network Network { get; }
    public RootUser Root { get; }
}

public sealed class Network(ushort port, string ipAddress)
{
    public ushort Port { get; } = port;
    public string IpAddress { get; } = ipAddress;
}

public sealed class RootUser(string name, string password, string token)
{
    public string Name { get; } = name;
    public string Password { get; } = password;
    public string Token { get; } = token;
}