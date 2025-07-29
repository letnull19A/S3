
namespace W2B.S3.Core.Interfaces;

public interface IControlModule
{
    public void Init();

    public void Start();

    public void TakeControl(IControlModule module);
    
    public void End();
}