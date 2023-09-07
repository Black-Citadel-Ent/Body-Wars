using BaseItems;
using UnityEngine;

namespace Player
{
    public class ElectricStunState : PlayerState
    {
        private float _effectTime;
        private Vector2 _baseVelocity;

        public override void BeginState(PlayerContext context)
        {
            base.BeginState(context);
            Context.ElectricStunVisual.SetActive(true);
            _effectTime = Time.fixedTime + 6.0f / 12.0f;
            _baseVelocity = Context.Body.velocity;
            Context.Body.angularVelocity = Random.Range(-180, 180);
        }

        public override void Update()
        {
            base.Update();
            var velMul = (_effectTime - Time.fixedTime).ClampLerp(0.5f, 0, 0.8f, 0.3f);
            Context.Body.velocity = _baseVelocity * velMul;
            if(Time.fixedTime >= _effectTime)
                Context.SetState(PlayerContext.StateName.UserControlled);
        }

        public override void EndState()
        {
            base.EndState();
            Context.ElectricStunVisual.SetActive(false);
            Context.Body.angularVelocity = 0;
        }
    }
}