using System.Text;
using W2B.S3.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace W2B.S3.Core.Configs;

public sealed class YAMLConfigLoader
{
    public static ConfigModel LoadConfig(string filePath)
    {
        var yamlContent = File.ReadAllText(filePath, Encoding.UTF8);
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        
        return deserializer.Deserialize<ConfigModel>(yamlContent);
    }
}