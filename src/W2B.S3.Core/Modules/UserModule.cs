using W2B.S3.Core.Configs;
using W2B.S3.Core.Interfaces;
using W2B.S3.Core.Models;

namespace W2B.S3.Core.Modules;

public sealed class UserModule(
    IReadOnlyDictionary<string, string> args,
    ConfigModel configs) 
  : IControlModule
{

  public void Init()
  { }

  public void Start()
  { }

  public void TakeControl(IControlModule module)
  { }

  public void End()
  { }

  public void Create()
  { }

  public void ChangeGroup()
  { }
}
