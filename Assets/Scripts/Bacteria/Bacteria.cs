using System;
using BaseItems;
using UnityEngine;

namespace Bacteria
{
    public class Bacteria : BaseBehavior, ILife
    {
        [SerializeField] private float speedBurst;
        [SerializeField] private float speedDrop;
        [SerializeField] private float maxLife;
        
        private BacteriaSetup _setup;
        private BacteriaContext _context;
        private float _life;

        public float SpeedBurst => speedBurst;
        public float SpeedDrop => speedDrop;

        public void RegisterSetup(BacteriaSetup setup)
        {
            _setup = setup;
        }

        private void Start()
        {
            _context = new BacteriaContext(this, _setup);
            _context.SetState(BacteriaContext.StateName.Move);
            _life = maxLife;
        }

        private void FixedUpdate()
        {
            _context.State.Update();
        }

        public void ApplyDamage(float amount, ILife.DamageType type)
        {
            _life -= amount;
        }
    }
}