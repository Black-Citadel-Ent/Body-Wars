using UnityEngine;

public class BlobDie : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;
    [SerializeField] private Animator animator;

    private void FixedUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Done"))
            Destroy(rootObject);
    }
}