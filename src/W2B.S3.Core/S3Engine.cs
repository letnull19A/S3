using W2B.S3.Core.Interfaces;
using W2B.S3.Core.Utils;

namespace W2B.S3.Core;

public sealed class S3Engine : IInitialize<S3Engine, string[]>
{
    private Dictionary<string, string> _parsedArgs;
    
    public S3Engine Init(string[] args)
    {
        var parser = new ArgsParser(args);
        _parsedArgs = parser.Parse();

        foreach (var arg in _parsedArgs)   
        {
            Console.WriteLine(arg.Key + " " + arg.Value);
        }

        return this;
    }

    public void CheckDbConnection()
    {
        
    }
}