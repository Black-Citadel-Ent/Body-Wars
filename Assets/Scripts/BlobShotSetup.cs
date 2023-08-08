using UnityEngine;

public class BlobShotSetup : MonoBehaviour
{
    [SerializeField] private float shotSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxAngleAdjust;

    public float ShotSpeed => shotSpeed;
    public float RotationSpeed => rotationSpeed;
    public float MaxAngleAdjust => maxAngleAdjust;
}