using BaseItems;
using UnityEngine;

namespace Virus
{
    public class Virus : BaseBehavior, ILife
    {
        [SerializeField] private float speed;
        [SerializeField] private float targetMinDistance;
        [SerializeField] private float targetMaxDistance;
        [SerializeField] private float shotSpeed;
        [SerializeField] private VirusShot shotTemplate;
        [SerializeField] private float maxLife;
        
        private VirusSetup _setup;
        private VirusContext _context;
        private float _life;
        private VirusSpawner.VirusSpawner _anchor;
        private VirusContext.StateName _startingState = VirusContext.StateName.Move;

        public float Speed => speed;
        public float TargetMinDistance => targetMinDistance;
        public float TargetMaxDistance => targetMaxDistance;
        public float ShotSpeed => shotSpeed;
        public VirusShot ShotTemplate => shotTemplate;
        public VirusSpawner.VirusSpawner Anchor => _anchor;

        public void RegisterSetup(VirusSetup setup)
        {
            _setup = setup;
        }

        public void ApplyDamage(float amount, ILife.DamageType type, float direction)
        {
            _life -= amount;
            _context.State.Damage();
            if(_life <= 0.0f)
                _context.SetState(VirusContext.StateName.Die);
            _setup.LifeBar.SetLife(_life / maxLife);
        }

        public void LockToSpawner(VirusSpawner.VirusSpawner spawner)
        {
            _startingState = VirusContext.StateName.Anchored;
            _anchor = spawner;
        }

        public void AnchorDead()
        {
            _context.SetState(VirusContext.StateName.Move);
        }

        private void Start()
        {
            _context = new VirusContext(_setup, this);
            _context.SetState(_startingState);
            _life = maxLife;
            _setup.LifeBar.SetLife(1.0f);
        }

        private void FixedUpdate()
        {
            _context.State.Update();
        }
    }
}