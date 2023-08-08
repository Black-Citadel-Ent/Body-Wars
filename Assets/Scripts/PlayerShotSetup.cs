using UnityEngine;

public class PlayerShotSetup : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float removeTime;

    public float Speed => speed;
    public float RemoveTime => removeTime;
}