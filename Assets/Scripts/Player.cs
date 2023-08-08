using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Inspector Properties
    [SerializeField] private PlayerSetup setup;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform shotTarget;
    [SerializeField] private LifeBar lifeBar;
    #endregion

    #region Input Storage
    private Vector2 _moveInput;
    private Vector2 _aimInput;
    #endregion
    
    #region Internal Private Fields
    private float _lastShotTime;
    private int _currentLife;
    #endregion
    
    #region Unity Loop
    private void Start()
    {
        _currentLife = setup.MaxLife;
    }

    private void Update()
    {
        lifeBar.UpdateLife(setup.MaxLife, _currentLife);
        if(_currentLife <= 0)
            Destroy(setup.gameObject);
    }

    private void FixedUpdate()
    {
        // Body Motion Calculations
        body.velocity = _moveInput * setup.Speed;
        var rotVel = 0.0f;
        var currentDir = body.gameObject.transform.rotation.eulerAngles.z.NormalizedAngle();
        if (!_moveInput.magnitude.NearZero())
        {
            var norm = _moveInput.normalized;
            var targetDir = (Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg).NormalizedAngle();
            var diff = (targetDir - currentDir).NormalizedAngle();
            rotVel = setup.TurnSpeed * Mathf.Sign(diff);
            if (Mathf.Abs(diff) < setup.TurnSpeed * Time.fixedDeltaTime)
                rotVel = diff / Time.fixedDeltaTime;
        }

        body.angularVelocity = rotVel;
        Vector2 vel = new Vector2(Mathf.Cos(currentDir * Mathf.Deg2Rad), Mathf.Sin(currentDir * Mathf.Deg2Rad));
        body.velocity = vel * (setup.Speed * _moveInput.magnitude);
        
        // Turret Motion Calculations
        if (!_aimInput.magnitude.NearZero())
        {
            var turretCurrentRot = turret.rotation.eulerAngles.z.NormalizedAngle();
            var turretNorm = _aimInput.normalized;
            var turretTargetRot = (Mathf.Atan2(turretNorm.y, turretNorm.x) * Mathf.Rad2Deg).NormalizedAngle();
            var turretDiff = (turretTargetRot - turretCurrentRot).NormalizedAngle();
            var turretFrameRot = setup.TurretTurnSpeed * Time.fixedDeltaTime * Mathf.Sign(turretDiff);
            if (Mathf.Abs(turretDiff) < setup.TurretTurnSpeed * Time.fixedDeltaTime)
                turretFrameRot = turretDiff;
            turret.rotation = Quaternion.Euler(0, 0, turretCurrentRot + turretFrameRot);
            
            // Fire Shot
            if (_lastShotTime + setup.ShotTime <= Time.fixedTime)
            {
                PlayerShot.StartShot(shotTarget, setup.ShotObject);
                _lastShotTime = Time.fixedTime;
            }
        }
        
    }
    #endregion

    #region Collision Loop
    public void BodyCollisionEnter(Collision2D other)
    {
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("EnemyHit"))
            print("Ship Hit Blob");
        else if (other.collider.gameObject.layer == LayerMask.NameToLayer("EnemyShot"))
            _currentLife -= 1;
    }
    #endregion
    
    #region Input Functions

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        _aimInput = context.ReadValue<Vector2>();
    }
    #endregion
}
