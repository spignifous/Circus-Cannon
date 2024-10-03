using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunState : GameState
{
    private int _timeMultiplier = 1;
    private int _life = 6;
    private int _size;

    private Camera _camera;
    private Coroutine _coroutine;

    public List<Vector2> SpawnPositions { get; private set; } = new List<Vector2>();

    public GameRunState(GameManager stateMachine, GameData data) : base("Game Run", stateMachine, data)
    {
        _camera = Camera.main;

        CreateSpawnPoints();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        _timeMultiplier = 1;

        //SpawnerBall();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Spawner Ball Manager
        SpawnRules();
        SpawnerBalls();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    #region SPAWN FUNCTIONS

    private void CreateSpawnPoints()
    {
        float height = 2f * _camera.orthographicSize;
        float width = height * _camera.aspect;

        Vector2 leftTop = new Vector2(_camera.transform.position.x - ((width / 2f) + 1f), _camera.transform.position.y + (height / 3f));
        Vector2 rightTop = new Vector2(_camera.transform.position.x + ((width / 2f) + 1f), _camera.transform.position.y + (height / 3f));
        Vector2 rightBottom = new Vector2(_camera.transform.position.x + ((width / 2f) + 1f), _camera.transform.position.y);
        Vector2 leftBottom = new Vector2(_camera.transform.position.x - ((width / 2f) + 1f), _camera.transform.position.y);

        SpawnPositions.Add(leftTop);
        SpawnPositions.Add(rightTop);
        SpawnPositions.Add(rightBottom);
        SpawnPositions.Add(leftBottom);
    }

    private void SpawnerBall()
    {
        int pointIndex = Random.Range(0, SpawnPositions.Count);
        Vector2 spawnPoint = SpawnPositions[pointIndex];

        // Pulling ball from the pool
        CircusBall ball = UnitManager.Instance.PullBall(spawnPoint, Quaternion.identity) as CircusBall;

        if (ball == null) return;

        // Choose Data Index
        int dataIndex = (pointIndex == 0 || pointIndex == 1) ? 1 : 0;

        // Choose Sprite Index
        int spriteIndex = (pointIndex == 0 || pointIndex == 1) ? 1 : 0;

        ball.SetData(dataIndex);
        ball.SetLife(_life);
        ball.SetSize(_size);
        ball.ApplyForceStart();
    }

    private void SpawnerBalls()
    {
        if (_coroutine != null) return;

        _coroutine = GameManager.StartCoroutine(SpawnerBallCoroutine());
    }

    private IEnumerator SpawnerBallCoroutine()
    {
        float time = Mathf.Clamp(_timeMultiplier / 4f, 0f, 5f);

        // wait for 1 second
        yield return new WaitForSeconds(.5f + time);

        SpawnerBall();

        _coroutine = null;
    }

    private void SpawnRules()
    {
        if (TimeReached(3f * _timeMultiplier))
        {
            _life = Random.Range(6, 6 * _timeMultiplier);
            _size = Random.Range(0, _timeMultiplier);
            _size = Mathf.Clamp(_size, 0, 2);

            _timeMultiplier++;
        }
    }

    #endregion
}
