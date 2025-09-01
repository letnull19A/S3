namespace W2B.S3.Core.Modules;

public sealed class UserModule(IReadonlyDictionary<string, string> args) 
  : IControlModule
{

  public void Init()
  { }

  public void Start()
  {

  }

  public void TakeControl(IControlModule module)
  { }

  public void End()
  { }

  public void Create()
  { }
}
