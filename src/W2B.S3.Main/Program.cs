using W2B.S3.Core;
using W2B.S3.RestAPI;
using W2B.S3.RestAPI;

namespace W2B.S3.Main;

public static class Program
{
    public static void Main(string[] args)
    {
        WelkomenMessage();

        var engine = new S3Engine(args);
        engine.Init();


        var engine = new S3Engine(args);
        engine.Init();

        engine.Start();
        engine.End();
        
        var t = new WebApiModule(args.ToArray());
        t.Start();
        
        var t = new WebApiModule(args.ToArray());
        t.Start();
    }

    private static void WelkomenMessage()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("""
                               ____        ____    ____________    __________
                              /   /\ ___  /   /\  /________   /\  /  ______  /\
                              /   / /  /\/   / / _\_______/__/ / /  /\____/ / /
                              /    /   //   / / /   /_______ \/ /  /\____/ / /
                              /_____//_____/ / /___________/\  /__________/ /
                              \_____\\_____\/  \___________\/  \__________\/
                          """);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}