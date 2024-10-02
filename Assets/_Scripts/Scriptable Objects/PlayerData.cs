using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Game/Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Cannon Settings")]
    public float SpeedCannon = 10f;

    [Space(5)]
    [Header("Bullet Settings")]
    public Bullet Bullet;
    public float FireRate = .1f;
    public float BulletSpeed = 15f;
}