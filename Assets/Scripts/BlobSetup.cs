using UnityEngine;

public class BlobSetup : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceFromOpponent;
    [SerializeField] private float maxDistanceFromOpponent;
    [SerializeField] private float waitTime;
    [SerializeField] private float shotGap;
    [SerializeField] private BlobShotSetup shotObject;
    [SerializeField] private float shotRotationSpeed;
    [SerializeField] private int life;
    [SerializeField] private float hitBounceTime;
    [SerializeField] private float hitBounceSpeed;
    [SerializeField] private float hitStunTime;
    [SerializeField] private GameObject deathEffect;

    public float Speed => speed;
    public float MinDistanceFromOpponent => minDistanceFromOpponent;
    public float MaxDistanceFromOpponent => maxDistanceFromOpponent;
    public float WaitTime => waitTime;
    public float ShotGap => shotGap;
    public BlobShotSetup ShotObject => shotObject;
    public float ShotRotationSpeed => shotRotationSpeed;
    public int Life => life;
    public float HitBounceTime => hitBounceTime;
    public float HitBounceSpeed => hitBounceSpeed;
    public float HitStunTime => hitStunTime;
    public GameObject DeathEffect => deathEffect;

    public BlobSpawnerSetup Spawner { get; set; }
}