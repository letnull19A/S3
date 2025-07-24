using W2B.S3.Core;

namespace W2B.S3.Main;

public static class Program
{
    public static void Main(string[] args)
    {
        var engine = new S3Engine()
            .Init(args);
    }
}