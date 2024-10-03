using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachine;

public class GameState : State
{
    protected GameManager GameManager { get; private set; }
    protected GameData Data { get; private set; }

    public GameState(string name, GameManager stateMachine, GameData data) : base(name, stateMachine)
    {
        this.GameManager = stateMachine;
        this.Data = data;
    }
}
