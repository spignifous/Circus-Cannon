
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Game.Pool;
using UnityEngine.UIElements;
using System.Drawing;

public class Ball : MonoBehaviour, IPoolable<Bullet>
{
    #region INSPECTOR VARIABLES

    [SerializeField] private Ball _ball;

    #endregion

    #region VARIABLES

    private ObjectPool<Bullet> _pool;

    private int _life = 6;
    private int _lifeInitial = 6;

    private int _size = 0;

    private float _gravityScale = 1f;
    private float _gravityScaleTarget = 1f;

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

        SetLife(Random.Range(6, 97));
        SetSize(Random.Range(0, 3));

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
            _rigidbody.AddTorque(_rigidbody.velocity.x * -5f);
        }
    }

    #endregion

    #region MOVING FUNCTIONS

    private void InitialMove()
    {
        var camera = Camera.main;

        if (camera == null) return;

        if (OutsideCamera())
        {
            _collider.isTrigger = true;

            _gravityScale = .1f;
            _gravityScaleTarget = _gravityScale;
        }

        Vector3 direction = (transform.position.x < camera.transform.position.x) ? new Vector3(1f, .8f): new Vector3(-1, .8f);
        float force = 1f + Random.Range(0f, 2f);

        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        _rigidbody.AddTorque(force * 20f);
    }

    private void UpdateGravityScale()
    {
        _gravityScale = Mathf.Lerp(_gravityScale, _gravityScaleTarget, .1f);
        _rigidbody.gravityScale = _gravityScale;
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
        else
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

    private void SizeDown()
    {
        if (_size > 0)
        {
            _size--;
        }
        else 
        {
            ///TODO: Destroy / Return to Pool
        }
    }

    private void UpdateScale()
    {
        float size;

        switch(_size)
        {
            default:
            case 0:
                size = .7f;
                break;
            case 1:
                size = 1;
                break;
            case 2:
                size = 1.5f;
                break;
            case 3:
                size = 2f;
                break;
        }

        transform.localScale *= size;
    }

    #endregion

    #region POOL FUNCTIONS

    public void ReturnToPool()
    {
        /// TODO: Destroy / Return to Pool and Create more balls
        /// 
        Destroy(this);

        if (_size > 0)
        {
            Ball b;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            position.x -= .5f;
            rotation.z -= .5f;

            b = Instantiate(_ball, position, transform.rotation);
            b.SetSize(_size - 1);

            position.x += .5f;
            rotation.z += .5f;

            b = Instantiate(_ball, position, transform.rotation);
            b.SetSize(_size - 1);
        }
    }

    public void SetPool(ObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    #endregion
}
