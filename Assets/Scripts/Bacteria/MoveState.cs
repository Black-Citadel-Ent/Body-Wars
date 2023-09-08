using BaseItems;
using UnityEngine;

namespace Bacteria
{
    public class MoveState : BacteriaState
    {
        private float _motionTime;

        public override void BeginState(BacteriaContext context) 
        {
            base.BeginState(context);
            _motionTime = 0;
        }

        public override void Update()
        {
            base.Update();
            _motionTime += Time.fixedDeltaTime;
            if (_motionTime >= 1.0f) _motionTime -= 1.0f;
            var dir = Context.Rotation.DegreesToVector2();
            Context.Body.velocity =
                dir * _motionTime.Lerp(0, 1, Context.SpeedBurst, Context.SpeedBurst * Context.SpeedDrop);
        }
    }
}