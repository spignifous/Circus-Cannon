
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachine;
using UnityEngine.Pool;

public class Unit : StateMachine
{
    #region UNIT

    public Unit unitObject => this;

    private ObjectPool<Unit> _pool;

    private UnitManager _manager;

    public void InitializeUnit(UnitManager manager, ObjectPool<Unit> pool)
    {
        _manager = manager;
        _pool = pool;
    }

    public void SetParent(Transform transform)
    {
        gameObject.transform.SetParent(transform);
    }

    #endregion


    #region POOLED

    private void OnDisable()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
