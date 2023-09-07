using BaseItems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Player : BaseBehavior, ILife
    {
        [SerializeField] private float speed;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float turretTurnSpeed;
        [SerializeField] private float shotsPerSecond;
        [SerializeField] private PlayerShot shotTemplate;
        [SerializeField] private float maxLife;
        [SerializeField] private float hitImmunity;

        private PlayerSetup _setup;
        private Vector2 _moveInput;
        private Vector2 _aimInput;
        private float _fireAtTime;
        private float _life;
        private float _hitImmuneTime;

        private PlayerContext _context;

        public Vector2 AimInput => _aimInput;
        public Vector2 MoveInput => _moveInput;
        public float TurnSpeed => turnSpeed;
        public float Speed => speed;
        public float TurretTurnSpeed => turretTurnSpeed;
        public PlayerShot ShotTemplate => shotTemplate;
        public float ShotsPerSecond => shotsPerSecond;

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            _aimInput = context.ReadValue<Vector2>();
        }

        public void RegisterSetup(PlayerSetup setup)
        {
            _setup = setup;
        }

        public void ApplyDamage(float amount, ILife.DamageType type)
        {
            if (Time.fixedTime > _hitImmuneTime)
            {
                _life -= amount;
                _setup.LifeBar.SetLife(_life / maxLife);
                if (type == ILife.DamageType.Electric)
                    _context.SetState(PlayerContext.StateName.ElectricStun);

                _hitImmuneTime = Time.fixedTime + hitImmunity;
            }
        }

        private void Start()
        {
            _context = new PlayerContext(_setup, this);
            _context.SetState(PlayerContext.StateName.UserControlled);
            _life = maxLife;
            _setup.LifeBar.SetLife(1.0f);
        }

        private void FixedUpdate()
        {
            _context.CurrentState.Update();
        }
    }
}