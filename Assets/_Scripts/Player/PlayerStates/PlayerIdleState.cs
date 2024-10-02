using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player stateMachine, PlayerData data, InputReader input) : base("Idle", stateMachine, data, input)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Player.MoveState.Move();

        if (Input.Touch)
        {
            Player.ChangeState(Player.MoveState);
        }
    }
}
