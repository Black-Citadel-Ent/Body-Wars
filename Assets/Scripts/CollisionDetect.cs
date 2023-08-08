using UnityEngine;
using UnityEngine.Events;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collision2D> collisionStart;
    [SerializeField] private UnityEvent<Collision2D> collisionStay;
    [SerializeField] private UnityEvent<Collision2D> collisionEnd;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        collisionStart.Invoke(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        collisionStay.Invoke(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        collisionEnd.Invoke(other);
    }
}
