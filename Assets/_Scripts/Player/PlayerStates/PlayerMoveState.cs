using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private Vector3 _positionOld;

    private float _timerSpawnBullet;

    private Vector3 _positionTarget;

    private Vector2 _vector2;
    private Vector3 _vector3;

    public PlayerMoveState(Player stateMachine, PlayerData data, InputReader input) : base("Move", stateMachine, data, input)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _timerSpawnBullet = 0;

        _positionOld = Player.transform.position;
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

        // Spin Wheel
        if (_positionOld.x != Player.transform.position.x)
        {
            var speed = (_positionOld.x - Player.transform.position.x) * 50f;
            Player.Wheel[0].SpinWheel(speed);
            Player.Wheel[1].SpinWheel(speed);

            _positionOld = Player.transform.position;
        }
    }

    private void Shoot()
    {
        if (_timerSpawnBullet == 0 || _timerSpawnBullet > Data.FireRate)
        {
            // Pulling bullet from the pool]
            Unit unit = UnitManager.Instance.PullBullet(Player.AimPivot.transform.position, Player.AimPivot.transform.rotation);
            if (unit)
            {
                (unit as Bullet).SetSpeed(Data.BulletSpeed);
            }

            // Pulling fire rate FX from the pool]
            _vector3.Set(Player.AimPivot.transform.position.x, Player.AimPivot.transform.position.y, Player.AimPivot.transform.position.z);
            unit = UnitManager.Instance.PullFireRate(_vector3, Player.AimPivot.transform.rotation);
            if (unit)
            {
                unit.SetParent(Player.transform);
            }

            _timerSpawnBullet = 0;

            // Audio Play
            AudioSystem.Instance.Play("Bullet");
        }

        _timerSpawnBullet += Time.deltaTime;
    }
}
