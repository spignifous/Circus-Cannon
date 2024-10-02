using UnityEngine;
using UnityEngine.Pool;

namespace Game.Pool
{
    public interface IPoolable<T> where T : class
    {
        void ReturnToPool(); 
        void SetPool(ObjectPool<T> pool);
    }
}