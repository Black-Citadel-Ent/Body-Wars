using BaseItems;
using UnityEngine;

namespace Player
{
    public abstract class MotionState : PlayerState
    {
        private Vector2 _impact;
        private float _impactStart, _impactEnd = float.MinValue;
        private Vector2 _motion = Vector2.zero;

        public override void Update()
        {
            base.Update();
            _motion = Vector2.zero;
            if (!Context.ApplyImpact.magnitude.NearZero())
            {
                _impact = Context.ApplyImpact;
                _impactStart = Time.fixedTime;
                _impactEnd = Time.fixedTime + 0.2f;
            }

            if (Time.fixedTime <= _impactEnd)
            {
                _motion = _impact * Time.fixedTime.Lerp(_impactStart, _impactEnd, 1.0f, 0.0f);
            }
        }

        protected Vector2 Motion => _motion;
    }
}