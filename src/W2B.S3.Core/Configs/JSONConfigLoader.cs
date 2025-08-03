using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using W2B.S3.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace W2B.S3.Core.Configs;

public sealed class JSONConfigLoader
{
    public static ConfigModel LoadConfig(string filePath)
    {
        var jsonContent = File.ReadAllText(filePath, Encoding.UTF8);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        return JsonSerializer.Deserialize<ConfigModel>(jsonContent, options) ??
               throw new InvalidDataException("Deserialization returned null");
    }
}