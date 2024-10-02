
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Game.Pool;

public class Ball : MonoBehaviour, IPoolable<Bullet>
{
    #region INSPECTOR VARIABLES

    [SerializeField] private Ball _ball;
    #endregion

    #region VARIABLES

    private ObjectPool<Bullet> _pool;

    private int _life = 30;
    private int _lifeInitial = 30;

    private int _facing = 1;
    private int _size = 3;

    private float _gravityScale = .8f;
    private float _gravityScaleTarget = .8f;

    #endregion

    #region COMPONENTS

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _renderer;

    private TextMeshPro _textMeshPro;

    #endregion

    #region UNITY FUNCTIONS

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider  = GetComponent<Collider2D>();
        _renderer  = GetComponent<SpriteRenderer>();

        _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        InitialMove();
        UpdateScale();
    }

    private void Update()
    {
        UpdateLifeGUI();
        UpdateGravityScale();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collider.isTrigger = false;
        _gravityScaleTarget = 1f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider)
        {
            if (_rigidbody.velocity.y > 1f)
            {
                _rigidbody.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
            }
            _rigidbody.AddTorque(_rigidbody.velocity.x * -5f);
        }
    }

    #endregion

    #region MOVING FUNCTIONS

    private void InitialMove()
    {
        var camera = Camera.main;

        if (camera == null) return;

        Vector3 direction = new Vector3(1f * _facing, 1.2f, 1f);
        float force = 1f + Random.Range(0f, 2f);

        if (OutsideCamera())
        {
            _collider.isTrigger = true;

            _gravityScale = .1f;
            _gravityScaleTarget = _gravityScale;

            direction = (transform.position.x < camera.transform.position.x) ? new Vector3(1f, .8f, 1f) : new Vector3(-1, .8f, 1f);
        }

        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        _rigidbody.AddTorque(force * 20f);
    }

    private void UpdateGravityScale()
    {
        _gravityScale = Mathf.Lerp(_gravityScale, _gravityScaleTarget, .1f);
        _rigidbody.gravityScale = _gravityScale;
    }

    private void SetFacing(int facing)
    {
        _facing = facing;
    }

    private bool OutsideCamera()
    {
        Camera camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        bool top = transform.position.y > camera.transform.position.y + (height / 2f);
        bool bottom = transform.position.y < camera.transform.position.y - (height / 2f);
        bool right = transform.position.x > camera.transform.position.x + (width / 2f);
        bool left = transform.position.x < camera.transform.position.x - (width / 2f);

        return top || bottom || right || left;
    }

    #endregion

    #region HIT FUNCTIONS

    public void TakeHit()
    {
        LifeDown();
    }

    #endregion

    #region LIFE FUNCTIONS

    public void SetLife(int life)
    {
        _life = life;
        _lifeInitial = life;
    }

    public void LifeDown()
    {
        if (_life > 0)
        {
            _life--;
        }

        if (_life == 0)
        {
            ReturnToPool();
        }
    }

    private void UpdateLifeGUI()
    {
        _textMeshPro.text = _life.ToString();
    }

    #endregion

    #region SIZE FUNCTIONS

    private void SetSize(int size)
    {
        _size = size;
    }


    private void UpdateScale()
    {
        float size;

        switch(_size)
        {
            default:
            case 0:
                size = .5f;
                break;
            case 1:
                size = .75f;
                break;
            case 2:
                size = 1f;
                break;
            case 3:
                size = 1.25f;
                break;
        }

        transform.localScale = Vector3.one * size;
    }

    #endregion

    #region POOL FUNCTIONS

    public void ReturnToPool()
    {
        /// TODO: Destroy / Return to Pool and Create more balls
        if (_size > 0)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            int life = _lifeInitial / 2;
            int size = _size - 1;

            position.x -= .5f;
            rotation.z -= .5f;

            Ball b1 = Instantiate(_ball, position, transform.rotation);
            b1.SetLife(life);
            b1.SetSize(size);

            position.x += .5f;
            rotation.z += .5f;

            Ball b2 = Instantiate(_ball, position, transform.rotation);
            b2.SetLife(life);
            b2.SetSize(size);
        }

        Destroy(gameObject);
    }

    public void SetPool(ObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    #endregion
}
