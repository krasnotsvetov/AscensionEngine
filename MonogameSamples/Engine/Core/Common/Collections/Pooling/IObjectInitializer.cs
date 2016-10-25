namespace MonogameSamples.Engine.Core.Common.Collections.Pooling
{
    public interface IObjectInitializer<T> where T : IPoolable
    {
        void Initialize(T t);
    }
}