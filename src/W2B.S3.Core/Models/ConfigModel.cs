using YamlDotNet.Serialization;

namespace W2B.S3.Core.Models;

public sealed class ConfigModel
{
    [YamlMember(Alias = "cluster")]
    public ClusterConfig Cluster { get; init; }
}

public sealed class ClusterConfig
{
    [YamlMember(Alias = "name")]
    public string Name { get; init; }

    [YamlMember(Alias = "mode")]
    public string Mode { get; init; }

    [YamlMember(Alias = "volume")]
    public string Volume { get; init; }

    [YamlMember(Alias = "root")]
    public RootUser Root { get; init; }

    [YamlMember(Alias = "network")]
    public NetworkConfig Network { get; init; }

    [YamlMember(Alias = "postgres")]
    public PostgresConfig Postgres { get; init; }
}

public sealed class RootUser
{
    [YamlMember(Alias = "user")]
    public string User { get; init; }

    [YamlMember(Alias = "password")]
    public string Password { get; init; }

    [YamlMember(Alias = "token")]
    public string Token { get; init; }
}

public sealed class NetworkConfig
{
    [YamlMember(Alias = "host")]
    public string Host { get; init; }

    [YamlMember(Alias = "port")]
    public ushort Port { get; init; }
}

public sealed class PostgresConfig
{
    [YamlMember(Alias = "user")]
    public string User { get; init; }

    [YamlMember(Alias = "password")]
    public string Password { get; init; }

    [YamlMember(Alias = "dataBase")]
    public string DataBase { get; init; }

    [YamlMember(Alias = "host")]
    public string Host { get; init; }

    [YamlMember(Alias = "port")]
    public ushort Port { get; init; }
}