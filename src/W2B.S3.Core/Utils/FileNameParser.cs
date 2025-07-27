namespace W2B.S3.Core.Utils;

public sealed class FileNameParser(string fileName)
{
    private readonly string _fileName = fileName;

    public string GetFileName()
        => _fileName[.._fileName.IndexOf(GetFileExtension(), StringComparison.Ordinal)];

    public string GetFileExtension() =>
        _fileName.Split('.')[^1];
    
}