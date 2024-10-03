using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : StateMachineStatic<GameManager>
{
    #region INSPECTOR VARIABLES

    [Header("Game Data")]
    [SerializeField] private GameData _data;

    public GameData Data => _data;

    #endregion

    #region STATES VARIABLES

    public GameStartState StartState { get; private set; }
    public GameRunState RunState { get; private set; }
    public GameLoseState LoseState { get; private set; }

    #endregion

    #region GAMEPLAY VARIABLES

    public bool PlayerLocked { get; private set; } = true;

    #endregion

    #region UI VARIABLES

    [SerializeField] private Button _retryButton;
    public Button RetryButton => _retryButton;

    [SerializeField] private Transform _scoreUI;
    public Transform ScoreUI => _scoreUI;

    #endregion

    #region UNITY CALLBACK FUNCTIONS

    protected override void Awake()
    {
        base.Awake();

        // States
        StartState = new GameStartState(this, _data);
        RunState = new GameRunState(this, _data);
        LoseState = new GameLoseState(this, _data);

        // Initial State
        SetInitialState(null, StartState);
    }

    #endregion

    #region STATEMACHINE CALLBACK FUNCTIONS

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    #endregion

    #region UI FUNCTIONS

    public void ButtonStartClick(Animator animator)
    {
        PlayerLocked = false;

        animator.Play("Start");

        StartState.PlayGame();

        _scoreUI.gameObject.SetActive(true);
    }

    public void ButtonRetryClick()
    {
        // Restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}