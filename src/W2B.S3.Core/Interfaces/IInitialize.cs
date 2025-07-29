namespace W2B.S3.Core.Interfaces;

public interface IInitialize<T, TU>
{
    public T Init(TU args);
}