using YamlDotNet.Serialization;

namespace W2B.S3.Core.Models;

public sealed class ConfigModel
{
    [YamlMember(Alias = "cluster")]
    public ClusterConfig Cluster { get; set; }
}

public sealed class ClusterConfig
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "mode")]
    public string Mode { get; set; }

    [YamlMember(Alias = "volume")]
    public string Volume { get; set; }

    [YamlMember(Alias = "root")]
    public RootUser Root { get; set; }

    [YamlMember(Alias = "network")]
    public NetworkConfig Network { get; set; }

    [YamlMember(Alias = "postgres")]
    public PostgresConfig Postgres { get; set; }
}

public sealed class RootUser
{
    [YamlMember(Alias = "user")]
    public string User { get; set; }

    [YamlMember(Alias = "password")]
    public string Password { get; set; }

    [YamlMember(Alias = "token")]
    public string Token { get; set; }
}

public sealed class NetworkConfig
{
    [YamlMember(Alias = "host")]
    public string Host { get; set; }

    [YamlMember(Alias = "port")]
    public ushort Port { get; set; }
}

public sealed class PostgresConfig
{
    [YamlMember(Alias = "user")]
    public string User { get; set; }

    [YamlMember(Alias = "password")]
    public string Password { get; set; }

    [YamlMember(Alias = "dataBase")]
    public string DataBase { get; set; }

    [YamlMember(Alias = "host")]
    public string Host { get; set; }

    [YamlMember(Alias = "port")]
    public ushort Port { get; set; }
}
  public string Token { get; } = token;
}