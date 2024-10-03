using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoseState : GameState
{
    public GameLoseState(GameManager stateMachine, GameData data) : base("Game Lose", stateMachine, data)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();

        GameManager.Instance.RetryButton.gameObject.SetActive(true);
    }
}
