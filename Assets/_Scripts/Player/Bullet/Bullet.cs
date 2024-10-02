using UnityEngine;
using UnityEngine.Pool;
using Game.Pool;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    private float _speed = 10f;
    
    private ObjectPool<Bullet> _pool;

    #region UNITY FUNCTIONS

    // Update is called once per frame
    void Update()
    {
        Move();

        // Return to the pool if the bullet leaves the screen
        if (OutsideCamera())
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out Ball ball))
        {
            ball.TakeHit();
            ReturnToPool();

            /// TODO: Add some impact effect here
        }
    }

    #endregion

    #region MOVE FUNCTIONS

    private void Move()
    {
        transform.position += (Vector3.up * _speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private bool OutsideCamera()
    {
        Camera camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        return (transform.position.y > camera.transform.position.y + (height / 2f) + 1f);
    }

    #endregion

    #region POOL FUNCTIONS

    public void ReturnToPool()
    {
        _pool.Release(this);
    }

    public void SetPool(ObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    #endregion
}
