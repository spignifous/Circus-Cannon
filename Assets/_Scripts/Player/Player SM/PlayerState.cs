using Game.StateMachine;

public class PlayerState : State
{
    protected Player Player { get; private set; }
    protected PlayerData Data { get; private set; }
    protected InputReader Input { get; private set; }

    public PlayerState(string name, Player stateMachine, PlayerData data, InputReader input) : base(name, stateMachine)
    {
        this.Player = stateMachine;
        this.Data = data;
        this.Input = input;
    }
}
