using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    private void FixedUpdate()
    {
        if (target)
        {
            var direction = target.transform.rotation.eulerAngles.z.DegreesToVector2();
            var targetPosition = (Vector2)target.transform.position + direction * distance;
            var motion = targetPosition - ((Vector2)transform.position);
            var distLeft = motion.magnitude;
            var frameMot = speed * Time.fixedDeltaTime;
                
            var pos = distLeft <= frameMot ? targetPosition : (Vector2)transform.position + (motion.normalized * frameMot);
            transform.position = new Vector3(pos.x, pos.y, -10);
        }
    }
}