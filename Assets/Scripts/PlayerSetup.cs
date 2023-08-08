using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float turretTurnSpeed;
    [SerializeField] private float shotTime;
    [SerializeField] private PlayerShotSetup shotObject;
    [SerializeField] private int maxLife;

    public float Speed => speed;
    public float TurnSpeed => turnSpeed;
    public float TurretTurnSpeed => turretTurnSpeed;
    public float ShotTime => shotTime;
    public PlayerShotSetup ShotObject => shotObject;
    public int MaxLife => maxLife;
}
