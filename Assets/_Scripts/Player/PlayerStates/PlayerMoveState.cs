using UnityEngine;
using UnityEngine.Pool;

public class PlayerMoveState : PlayerState
{
    private ObjectPool<Bullet> _bulletPool;
    private float _timerSpawnBullet;

    private Vector3 _positionTarget;

    public PlayerMoveState(Player stateMachine, PlayerData data, InputReader input) : base("Move", stateMachine, data, input)
    {
        // Create bullet pool
        _bulletPool = new ObjectPool<Bullet>(CreateBullet, PullBulletCallback, PushBulletCallback, DestroyBulletCallback, true, 10, 20);
    }

    public override void OnEnter()
    {
        _timerSpawnBullet = 0;
    }

    public override void DoChecks()
    {
        base.DoChecks();
            
        _positionTarget = Input.TouchPositionWorld();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Move
        Move();

        // Shoot
        Shoot();

        // Change State
        if (Input.TouchReleased)
        {
            Player.ChangeState(Player.IdleState);
        }
    }

    public void Move()
    {
        Vector3 position = Player.transform.position;
        position.x = Mathf.Lerp(Player.transform.position.x, _positionTarget.x, 1 - Mathf.Pow(2, -Time.deltaTime * Data.SpeedCannon));

        Player.transform.position = position;
    }

    private void Shoot()
    {
        if (_timerSpawnBullet == 0 || _timerSpawnBullet > Data.FireRate)
        {
            _bulletPool.Get();
            _timerSpawnBullet = 0;
        }

        _timerSpawnBullet += Time.deltaTime;
    }

    #region BULLET POOL

    private Bullet CreateBullet()
    {
        // Spawn new instance pf the bullet
        Bullet bullet = GameObject.Instantiate(Data.Bullet, Player.AimPivot.transform.position, Player.AimPivot.transform.rotation);

        // Assign the bullet'a pool
        bullet.SetPool(_bulletPool);

        // Set speed bullet
        bullet.SetSpeed(Data.BulletSpeed);

        return bullet;
    }

    private void PullBulletCallback(Bullet bullet)
    {
        bullet.transform.position = Player.AimPivot.transform.position;
        bullet.transform.rotation = Player.AimPivot.transform.rotation;

        // Update speed bullet
        bullet.SetSpeed(Data.BulletSpeed);

        // Activate
        bullet.gameObject.SetActive(true);
    }

    private void PushBulletCallback(Bullet bullet)
    {
        // Desactivate
        bullet.gameObject.SetActive(false);
    }

    private void DestroyBulletCallback(Bullet bullet)
    {
        // Destroy
        GameObject.Destroy(bullet.gameObject);
    }

    #endregion
}
