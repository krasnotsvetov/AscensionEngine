namespace Ascension.Engine.Core.Common.Collections.Pooling
{
    public interface IPool<T>
    {
        /// <summary>
        /// Add an object to pool
        /// </summary>
        /// <param name="t"></param>
        void Add(T t);

        /// <summary>
        /// Return value which contains in pool. If pool is empty, the new object will be created and returned.
        /// </summary>
        /// <returns></returns>
        T Get();
    }
}