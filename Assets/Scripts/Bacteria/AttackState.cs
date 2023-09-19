using BaseItems;
using UnityEngine;

namespace Bacteria
{
    public class AttackState : BacteriaState
    {
        private float _attackStart;
        private float _attackTime;
        private float _hitMoment;
        private bool _hit;
        
        public override void BeginState(BacteriaContext context)
        {
            base.BeginState(context);
            Context.Visual(BacteriaContext.StateName.Attack).SetActive(true);
            _attackStart = Time.fixedTime;
            _attackTime = Time.fixedTime + 6.0f / 12.0f;
            _hitMoment = Time.fixedTime + 3.0f / 12.0f;
            _hit = false;
        }

        public override void Update()
        {
            base.Update();
            if (Time.fixedTime >= _attackTime)
                Context.SetState(BacteriaContext.StateName.Move); 
            else if (Time.fixedTime >= _hitMoment)
            {
                Context.Body.velocity = Vector2.zero;
                if (!_hit)
                {
                    _hit = true;
                    if (Context.TargetInRange)
                    {
                        var hit = Context.Target.GetComponent<ILife>();
                        hit?.ApplyDamage(Context.AttackDamage, ILife.DamageType.Impact, Context.Body.rotation);
                    }
                }
            }
            else
                Context.Body.velocity = Context.Body.rotation.DegreesToVector2() * (Context.SpeedBurst * 2);
        }

        public override void EndState()
        {
            base.EndState();
            Context.Visual(BacteriaContext.StateName.Attack).SetActive(false);
            Context.Body.velocity = Vector2.zero;
        }
    }
}