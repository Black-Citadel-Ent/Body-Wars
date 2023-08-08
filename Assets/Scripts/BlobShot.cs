using System.Collections.Generic;
using UnityEngine;

public class BlobShot : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private BlobShotSetup rootObject;
    [SerializeField] private Transform rotationRoot;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator ani;
    #endregion
        
    #region Internal Private Fields
    private Transform _target;
    private float _maxTime;
    private float _initialTime;
    private float _direction;
    private bool _exploded;
    private float _explodeStartTime;

    private static readonly int ExplodeHash = Animator.StringToHash("Explode");
    #endregion

    #region Private Functions
    private void ResetShot((Vector3, Transform) info)
    {
        rootObject.transform.position = info.Item1;
        _target = info.Item2;
        var vecDiff = (info.Item2.transform.position - info.Item1).normalized;
        _direction = Mathf.Atan2(vecDiff.y, vecDiff.x) * Mathf.Rad2Deg;
        _maxTime = ((_target.position - info.Item1).magnitude * 1.25f) / rootObject.ShotSpeed;
        _initialTime = Time.time;
        _exploded = false;
    }

    private void Explode()
    {
        _exploded = true;
        ani.SetTrigger(ExplodeHash);
        _explodeStartTime = Time.time;
    }
    #endregion

    #region Unity Loop
    private void Update()
    {
        if (!_exploded)
        {
            var rot = rotationRoot.rotation.eulerAngles.z;
            rot += (rootObject.RotationSpeed * Time.deltaTime).NormalizedAngle();
            rotationRoot.rotation = Quaternion.Euler(0, 0, rot);

            if (Time.time - _initialTime >= _maxTime)
                Explode();
        }
        else
        {
            if (Time.time - _explodeStartTime < 0.1f) return;
            if (!ani.GetCurrentAnimatorStateInfo(0).IsTag("Explode"))
                ReturnShot(rootObject);
        }
    }

    private void FixedUpdate()
    {
        if (_exploded)
        {
            body.velocity = Vector2.zero;
            return;
        }

        if (!_target)
        {
            Explode();
            return;
        }

        var vecDiff = (_target.position - rootObject.transform.position).normalized;
        var targetAngle = Mathf.Atan2(vecDiff.y, vecDiff.x) * Mathf.Rad2Deg;
        var maxChange = rootObject.MaxAngleAdjust * Time.fixedDeltaTime;
        var diff = (targetAngle - _direction).NormalizedAngle();
        if (Mathf.Abs(diff) < maxChange)
            _direction = targetAngle;
        else
            _direction += maxChange * Mathf.Sign(diff);
            
        body.MovePosition(body.position + (_direction.DegreesToVector2() * (rootObject.ShotSpeed * Time.fixedDeltaTime)));
    }
    #endregion

    #region Collision Loop
    private void OnCollisionEnter2D(Collision2D other)
    {
        Explode();
    }
    #endregion
        
    #region Queue
    private static readonly Queue<BlobShotSetup> ShotQueue = new ();

    public static void StartShot(Vector3 position, BlobShotSetup template)
    {
        if (!ShotQueue.TryDequeue(out var shotObj))
            shotObj = Instantiate(template);
        shotObj.gameObject.SetActive(true);
        shotObj.BroadcastMessage("ResetShot", (position, GameObject.FindWithTag("Player").transform));
    }

    private static void ReturnShot(BlobShotSetup shot)
    {
        shot.gameObject.SetActive(false);
        ShotQueue.Enqueue(shot);
    }
    #endregion
}