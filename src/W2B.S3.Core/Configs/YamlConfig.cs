namespace W2B.S3.Core.Configs;

public sealed class YamlConfig
{
    private readonly string _filePath = string.Empty;
    
    public void Read()
    {
        if (!File.Exists(Directory.GetCurrentDirectory() + _filePath))
            throw new FileNotFoundException("file not found");
    }

    public void GetRootConfirg()
    {
    }

    public void Get()
    {
    }
}