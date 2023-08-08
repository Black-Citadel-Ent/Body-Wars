using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private PlayerShotSetup setup;
    [SerializeField] private Rigidbody2D body;
    #endregion
        
    #region Private Internal Fields
    private float _time;
    private Vector2 _wallImpact;
    #endregion
        
    #region Unity Loop Functions
    private void Update()
    {
        if (_time >= setup.RemoveTime)
            ReturnShot(setup);
    }

    private void FixedUpdate()
    {
        var rotRad = body.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        var rotVec = new Vector2(Mathf.Cos(rotRad), Mathf.Sin(rotRad));
        body.velocity = rotVec * setup.Speed;
        _time += Time.fixedDeltaTime;
    }

    private void ResetShot(Transform t)
    {
        setup.transform.SetPositionAndRotation(t.position, t.rotation);
        _time = 0;
    }
    #endregion

    #region Collision Loop
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("FlexWall"))
            _wallImpact = other.contacts[0].point;
        ReturnShot(setup);
    }
    #endregion
        
    #region Shot Queue Setup
    private static readonly Queue<PlayerShotSetup> ShotQueue = new ();

    public static void StartShot(Transform setup, PlayerShotSetup template)
    {
        if (!ShotQueue.TryDequeue(out var shot))
            shot = Instantiate(template).GetComponent<PlayerShotSetup>();
        shot.gameObject.SetActive(true);
        shot.gameObject.BroadcastMessage("ResetShot", setup);
    }

    private static void ReturnShot(PlayerShotSetup s)
    {
        ShotQueue.Enqueue(s);
        s.gameObject.SetActive(false);
    }
    #endregion
}