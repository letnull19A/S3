namespace W2B.S3.Core.Utils;

public sealed class ArgsParser(IReadOnlyList<string> args)
{
    public Dictionary<string, string> Parse()
    {
        if (args.Count % 2 != 0) 
            throw new Exception("arguments cannot be parsed because values incorrect!");

        var argsDictionary = new Dictionary<string, string>();

        for (var i = 0; i < args.Count; i += 2) {
            argsDictionary.Add(args[i], args[i + 1]);
        }

        return argsDictionary;
    }
}