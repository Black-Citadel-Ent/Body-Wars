using BaseItems;
using UnityEngine;

namespace Bacteria
{
    public class Bacteria : BaseBehavior, ILife
    {
        [SerializeField] private float speedBurst;
        [SerializeField] private float speedDrop;
        [SerializeField] private float maxLife;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float attackDamage;
        
        private BacteriaSetup _setup;
        private BacteriaContext _context;
        private float _life;
        private GameObject _target;
        private bool _targetInRange;

        public float SpeedBurst => speedBurst;
        public float SpeedDrop => speedDrop;
        public float TurnSpeed => turnSpeed;
        public GameObject Target => _target;
        public bool TargetInRange => _targetInRange;
        public float AttackDamage => attackDamage;

        public void RegisterSetup(BacteriaSetup setup)
        {
            _setup = setup;
        }

        private void Start()
        {
            _context = new BacteriaContext(this, _setup);
            _context.SetState(BacteriaContext.StateName.Move);
            _life = maxLife;
            _setup.LifeBar.SetLife(1.0f);
        }

        private void FixedUpdate()
        {
            _context.State.Update();
        }

        public void ApplyDamage(float amount, ILife.DamageType type, float direction)
        {
            _life -= amount;
            _setup.LifeBar.SetLife(_life / maxLife);
            if(_life <= 0)
                _context.SetState(BacteriaContext.StateName.Die);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.otherCollider.gameObject.layer == LayerMask.NameToLayer("EnemyDetect"))
                _target = other.gameObject;
            else if (other.otherCollider.gameObject.layer == LayerMask.NameToLayer("EnemyShot"))
            {
                _target = other.gameObject;
                _targetInRange = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.otherCollider.gameObject.layer == LayerMask.NameToLayer("EnemyShot"))
                _targetInRange = false;
        }
    }
}