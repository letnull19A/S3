using W2B.S3.Core.Interfaces;

namespace W2B.S3.Core.Modules;

public sealed class RootUser : IControlModule<RootUser, Dictionary<string, string>>
{
    public RootUser Init(Dictionary<string, string> args)
    {
        return this;
    }

    public void Start()
    {
        throw new NotImplementedException();
    }

    public void TakeControl(IControlModule<RootUser, Dictionary<string, string>> module)
    {
        throw new NotImplementedException();
    }

    public void End()
    {
        throw new NotImplementedException();
    }
}