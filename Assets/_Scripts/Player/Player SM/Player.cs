using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.StateMachine;
using UnityEngine.InputSystem.LowLevel;

public class Player : StateMachine
{
    #region INSPECTOR VARIABLES

    [Header("Player Data")]
    [SerializeField] private PlayerData _data;

    //public PlayerData Data => _data;

    [Header("Player Input")]
    [SerializeField] private InputReader _input;

    //public InputReader Input => _input; }

    [Header("Aim Pivot")]
    [SerializeField] private Transform _aimPivot;
    public Transform AimPivot => _aimPivot;

    #endregion


    #region STATES VARIABLES

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDieState DieState { get; private set; }

    #endregion

    #region COMPONENTS VARIABLES

    public SpriteRenderer SpriteRenderer { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public BoxCollider2D BoxCollider { get; private set; } 
    public Animator Animator { get; private set; }
    public PlayerWheel[] Wheel { get; private set; }

    #endregion

    #region UNITY CALLBACK FUNCTIONS

    private void Awake()
    {
        // Components
        BoxCollider = GetComponent<BoxCollider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponentInChildren<Animator>();

        Wheel = GetComponentsInChildren<PlayerWheel>();

        // States
        IdleState = new PlayerIdleState(this, _data, _input);
        MoveState = new PlayerMoveState(this, _data, _input);
        DieState = new PlayerDieState(this, _data, _input);

        // Initial State
        SetInitialState(null, IdleState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CircusBall ball))
        {
            ChangeState(DieState);
        }
    }

    #endregion
}
