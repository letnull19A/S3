
namespace W2B.S3.Core.Interfaces;

public interface IControlModule<T, TU>
{
    public T Init(TU args);

    public void Start();

    public void TakeControl(IControlModule<T, TU> module);
    
    public void End();
}