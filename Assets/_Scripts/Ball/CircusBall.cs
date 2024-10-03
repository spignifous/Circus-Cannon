using TMPro;
using UnityEngine;

public class CircusBall : Unit
{
    #region INSPECTOR VARIABLES

    [Header("Settings")]
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private BallData _data;

    [Space(5)]
    [SerializeField] private BallData[] _datas;

    [Space(5)]
    [SerializeField] private Sprite[] _sprites;

    #endregion

    #region VARIABLES

    private Vector2 _vector2;
    private Vector3 _vector3;

    private int _facing = 1;
    
    private int _life = 6;
    private int _lifeInitial = 6;

    private int _size = 0;

    private bool _shaky;
    private float _shakyMagnitude;
    private float _shakyTimer = 0f;
    private float _shakyDuration = .2f;

    #endregion

    #region COMPONENTS

    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    private SpriteRenderer _renderer;

    private TextMeshPro _textMeshPro;

    #endregion

    #region UNITY FUNCTIONS

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();

        _renderer = GetComponentInChildren<SpriteRenderer>();
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        FacingStart();
        SetSprite(GetRandomSpriteIndex());
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLifeGUI();
        UpdateFacing();
        UpdateShaky();

        if (!_collider.isTrigger)
        {
            HorizontalMovement();
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetGravityScale(_data.GravityScale);
        _collider.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsTouchingGround())
        {
            Jump();
        }

        if (IsTouchingWall())
        {
            WallImpulse();
        }
    }

    #endregion

    #region MOVING FUNCTIONS

    private void HorizontalMovement(float lerpAmount = 1f)
    {
        // Calcule a dire��o em que queremos nos mover e a velocidade desejada
        float targetSpeed = _facing * _data.maxSpeed;

        // Podemos reduzir o controle usando Lerp(), isso suaviza as mudan�as de dire��o e velocidade
        targetSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, lerpAmount);

        #region Calculate acceleration Rate

        float accelerationRate;

        // Obt�m um valor de acelera��o com base se estamos acelerando (inclui curvas) 
        // ou tentando desacelerar (parar). Al�m de aplicar um multiplicador se estivermos no ar.
        accelerationRate = _data.accelerationAmount * _data.acceleration;

        #endregion

        // Calcule a diferen�a entre a velocidade atual e a velocidade desejada
        float speedDifference = targetSpeed - _rigidbody.velocity.x;

        // Calcule a for�a ao longo do eixo x para aplicar ao jogador
        float movement = speedDifference * accelerationRate;

        // Converta isso em um vetor e aplique ao corpo r�gido
        _rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Jump()
    {
        SetGravityScale(_data.GravityScale);

        #region PERFORM JUMP

        // Aumentar a for�a aplicada se estiver caindo
        // Isso serve para que o salto sempore seja na mesma altura 
        // (definir a velocidade Y do jogador como 0 de antem�o provavelmente funcionar� da mesma forma, mas prefiro usar assim
        float force = _data.JumpStrenghtMax;
        if (_rigidbody.velocity.y < 0 || _rigidbody.velocity.y > _data.JumpStrenghtMax)
        {
            force -= _rigidbody.velocity.y;
        }

        // Aplicar for�a de salto
        _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        _rigidbody.AddTorque(force * _facing);

        #endregion
    }

    private void WallImpulse()
    {
        _rigidbody.AddForce(Vector2.left * _data.wallImpulse * _facing, ForceMode2D.Force);
        _rigidbody.AddTorque(_data.wallImpulse * _facing);
    }

    public void ApplyForceStart()
    {
        var camera = Camera.main;

        if (camera == null) return;

        // Verificar se est� fora da tela
        if (OutsideCamera())
        {
            FacingStart(); 
            SetGravityScale(0);

            _collider.isTrigger = true;

            float force = 1.5f + Random.Range(0f, 2f);
            float angle = (_facing == -1) ? 150f : 30f;

            _rigidbody.AddForceAtAngle(force, angle, ForceMode2D.Impulse);
            _rigidbody.AddTorque(force * 20f * _facing);
        }
    }

    public void ApplyImpulse(float force, float angle)
    {
        _rigidbody.AddForceAtAngle(force, angle, ForceMode2D.Impulse);
        _rigidbody.AddTorque(force * _facing);
    }

    #endregion

    #region COLLISION FUNCTIONS

    public bool InsideGroundLayer() 
    {
        return Physics2D.CircleCast(_collider.bounds.center, _collider.radius, Vector2.zero, 0f, _whatIsGround);
    }

    public bool IsTouchingGround()
    {
        return !InsideGroundLayer() && Physics2D.CircleCast(_collider.bounds.center, _collider.radius, Vector2.down, .1f, _whatIsGround);
    }

    public bool IsTouchingWall()
    {
        Vector2 direction = _facing * Vector2.right;

        return !InsideGroundLayer() && Physics2D.CircleCast(_collider.bounds.center, _collider.radius , direction, .05f, _whatIsGround);
    }

    #endregion

    #region HIT FUNCTIONS

    public void TakeHit()
    {
        LifeDown();
        Shaky(.01f, .5f);
    }

    public void Shaky(float magnitude, float duration)
    {
        _shakyMagnitude = magnitude;
        _shakyDuration = duration;
        _shaky = true;
    }

    private void UpdateShaky()
    {
        if (_shaky)
        {
            _shakyTimer += Time.deltaTime;

            if (_shakyTimer >= _shakyDuration)
            {
                _shakyTimer = 0;
                _shaky = false;
            }

            // Shaky
            float x = Random.Range(-_shakyMagnitude, _shakyMagnitude);
            float y = Random.Range(-_shakyMagnitude, _shakyMagnitude);

            _vector2.Set(x, y);

            _renderer.transform.localPosition = transform.localPosition * _vector2;
        }
    }

    public void Died()
    {
        if (_size > 0)
        {
            int life = _lifeInitial / 2;
            int size = _size - 1;

            CircusBall ball;
            int facing;
            float angle = 90f;

            // Create double balls
            for(var i = 0; i < 2; i++)
            {
                facing = ( i == 0) ? -1 : 1;

                ball = UnitManager.Instance.PullBall(transform.position, transform.rotation) as CircusBall;

                ball.SetFacing(facing);
                ball.SetLife(life);
                ball.SetSize(size);
                ball.SetSprite(GetRandomSpriteIndex());
                ball.ApplyImpulse(7f, angle + (45f * facing));
            }
        }

        // Pulling ExplosionFX from the pool]
        UnitManager.Instance.PullExplosion(transform.position, transform.rotation);

        // Audio Play
        AudioSystem.Instance.Play("Ball Pop");
        AudioSystem.Instance.Play("Pop");
        AudioSystem.Instance.Play("Poof");

        // Return to pool
        Disable();
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
            Died();
        }
    }

    private void UpdateLifeGUI()
    {
        _textMeshPro.text = _life.ToString();
    }

    #endregion

    #region SIZE FUNCTIONS

    public void SetSize(int size)
    {
        _size = size;
    }

    #endregion

    #region OTHER FUNCTIONS

    public void SetData(int index)
    {
        if (index > _datas.Length) return;

        _data = _datas[index];
    }

    private void SetSprite(int index)
    {
        if (index > _sprites.Length) return;

        _renderer.sprite = _sprites[index];
    }
    public int GetRandomSpriteIndex()
    {
        switch(_size)
        {
            case 0: return Random.Range(0, 3);
            case 1: return Random.Range(4, 7);
            default:
            case 2: return Random.Range(8, 11);
        }
    }


    private void FacingStart()
    {
        Camera camera = Camera.main;

        _facing = (transform.position.x < camera.transform.position.x) ? 1 : -1;
    }

    private void SetFacing(int facing)
    {
        _facing = facing;
    }

    private void UpdateFacing()
    {
        _facing = (_rigidbody.velocity.x > 0) ? 1 : -1;

        if (_rigidbody.velocity.x == 0 && IsTouchingWall())
        {
            _facing *= -1;
        }
    }

    private void SetGravityScale(float gavityScale)
    {
        _rigidbody.gravityScale = gavityScale;
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
}