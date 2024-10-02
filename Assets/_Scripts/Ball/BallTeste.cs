using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class BallTeste : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private BallData _data;

        #endregion

        #region VARIABLES

        private int _facing = 1;
        private Vector2 _vector2;
        private Vector3 _vector3;

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
            _renderer = GetComponent<SpriteRenderer>();
        }


        // Start is called before the first frame update
        private void Start()
        {
            InitialMove();
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateFacing();

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
            // Calcule a direção em que queremos nos mover e a velocidade desejada
            float targetSpeed = _facing * _data.maxSpeed;

            // Podemos reduzir o controle usando Lerp(), isso suaviza as mudanças de direção e velocidade
            targetSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, lerpAmount);

            #region Calculate acceleration Rate

            float accelerationRate;

            // Obtém um valor de aceleração com base se estamos acelerando (inclui curvas) 
            // ou tentando desacelerar (parar). Além de aplicar um multiplicador se estivermos no ar.
            accelerationRate = _data.accelerationAmount * _data.acceleration;

            #endregion

            // Calcule a diferença entre a velocidade atual e a velocidade desejada
            float speedDifference = targetSpeed - _rigidbody.velocity.x;

            // Calcule a força ao longo do eixo x para aplicar ao jogador
            float movement = speedDifference * accelerationRate;

            // Converta isso em um vetor e aplique ao corpo rígido
            _rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        private void Jump()
        {
            SetGravityScale(_data.GravityScale);

            #region PERFORM JUMP

            // Aumentar a força aplicada se estiver caindo
            // Isso serve para que o salto sempore seja na mesma altura 
            // (definir a velocidade Y do jogador como 0 de antemão provavelmente funcionará da mesma forma, mas prefiro usar assim
            float force = _data.JumpStrenghtMax;
            if (_rigidbody.velocity.y < 0 || _rigidbody.velocity.y > _data.JumpStrenghtMax)
            {
                force -= _rigidbody.velocity.y;
            }

            // Aplicar força de salto
            _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            _rigidbody.AddTorque(force * _facing);

            #endregion
        }

        private void WallImpulse()
        {
            _rigidbody.AddForce(Vector2.left * _data.wallImpulse * _facing, ForceMode2D.Force);
            _rigidbody.AddTorque(_data.wallImpulse * _facing);
        }

        private void InitialMove()
        {
            SetFacingStart();
            SetGravityScale(_data.GravityScale);

            var camera = Camera.main;

            if (camera == null) return;

            _vector3.Set(1f * _facing, 1.5f, 1f);
            Vector3 direction = _vector3;

            float force = 1.5f + Random.Range(0f, 2f);

            // Verificar se está fora da tela
            if (OutsideCamera())
            {
                SetGravityScale(0);
                _collider.isTrigger = true;

                _vector3.Set(1f * _facing, .5f, 1f);
                direction = _vector3;
            }

            _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
            _rigidbody.AddTorque(force * 20f * _facing);
        }

        #endregion

        #region CHECK FUNCTIONS

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

        #region OTHER FUNCTIONS

        private void SetFacingStart()
        {
            Camera camera = Camera.main;

            _facing = (transform.position.x < camera.transform.position.x) ? 1 : -1;
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
}
