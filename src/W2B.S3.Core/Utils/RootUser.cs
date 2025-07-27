using W2B.S3.Core.Interfaces;

namespace W2B.S3.Core.Utils;

public sealed class RootUser : IInitialize<RootUser, Dictionary<string, string>>
{
    public RootUser Init(Dictionary<string, string> args)
    {
        return this;
    }
}