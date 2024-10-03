using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameStartState : GameState
{
    public GameStartState(GameManager stateMachine, GameData data) : base("Game Start", stateMachine, data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 
        GameManager.Instance.Data.ResetScore();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Fazer ajustes iniciais do jogo
        // Depois mudar estado para Run
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void PlayGame()
    {
        GameManager.ChangeState(GameManager.RunState);
    }
}