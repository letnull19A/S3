using W2B.S3.Core;

namespace W2B.S3.Main;

public static class Program
{
    public static void Main(string[] args)
    {
        WelkomenMessage();
        
        var engine = new S3Engine()
            .Init(args);
        
        engine.Start();
        engine.End();
    }

    private static void WelkomenMessage()
    {
        Console.WriteLine("""
             ____        ____    ____________    ___________
            /   /\ ___  /   /\  /________   /\  /  /\_/    /\
            /   / /  /\/   / / _\_______/__/ / /  ___    /\\/
            /    /   //   / / /   /_______ \/ /  /\_/    /\
            /_____//_____/ / /___________/\  /_________/\\/
            \_____\\_____\/  \___________\/  \_________\/
        """);
    }
}