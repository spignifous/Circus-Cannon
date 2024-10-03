using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(Player stateMachine, PlayerData data, InputReader input) : base("Die", stateMachine, data, input)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Pulling ExplosionFX from the pool]
        UnitManager.Instance.PullExplosion(Player.transform.position, Player.transform.rotation);
        UnitManager.Instance.PullExplosion(Player.transform.position + Vector3.left, Player.transform.rotation);
        UnitManager.Instance.PullExplosion(Player.transform.position + Vector3.right, Player.transform.rotation);
        UnitManager.Instance.PullExplosion(Player.transform.position + Vector3.up, Player.transform.rotation);
        UnitManager.Instance.PullExplosion(Player.transform.position + Vector3.down, Player.transform.rotation);

        GameObject.Destroy(Player.gameObject);

        // State Lose
        GameManager.Instance.ChangeState(GameManager.Instance.LoseState);

        // Audio Play
        AudioSystem.Instance.Play("Death");
        AudioSystem.Instance.Play("Death Hit");
        AudioSystem.Instance.Play("Ball Pop Death");
        AudioSystem.Instance.Play("Bullet Pop Hit");
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
