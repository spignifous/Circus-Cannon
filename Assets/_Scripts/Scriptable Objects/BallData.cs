using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ball Data", menuName = "Game/Data/Ball Data")]
public class BallData : ScriptableObject
{
    #region GRAVITY

    public float GravityStrength { get; private set; }
    public float GravityScale { get; private set; }

    #endregion

    #region MOVEMENT

    [Space(5)]
    [Header("Move")]
    public float maxSpeed = 2.5f;
    public float wallImpulse = 10;
    [Range(0f, 1)] public float acceleration = .3f; 

    public float accelerationAmount { get; private set; } 

    #endregion

    #region JUMP

    [Header("Jump")]
    [SerializeField] private float _jumpHeightMax = 10f;
    [SerializeField] private float _jumpTimeToApex = 1.5f;

    public float JumpStrenghtMax { get; private set; }

    #endregion

    private void Awake()
    {
        Validate();
    }

    private void OnValidate()
    {
        Validate();
    }

    private void Validate()
    {
        CalculateJump();
        CalculateAcceleration();
        ClampSpeed();
    }

    private void CalculateJump()
    {
        //Calcule a força da gravidade usando a fórmula(gravidade = 2 * jumpHeight / timeToJumpApex ^ 2)
        GravityStrength = -(2 * _jumpHeightMax) / (_jumpTimeToApex * _jumpTimeToApex);

        // Calcule a escala de gravidade do corpo rígido em relação ao valor da gravidade da Unity)
        GravityScale = GravityStrength / Physics2D.gravity.y;

        // Calcule jumpStrenghtMax
        JumpStrenghtMax = Mathf.Sqrt(-2 * GravityStrength * _jumpHeightMax);
    }

    private void CalculateAcceleration()
    {
        accelerationAmount = ((1 / Time.fixedDeltaTime) * acceleration) / maxSpeed;
    }

    private void ClampSpeed()
    {
        #region Variable Ranges
        acceleration = Mathf.Clamp(acceleration, 0.01f, maxSpeed);
        #endregion
    }
}
