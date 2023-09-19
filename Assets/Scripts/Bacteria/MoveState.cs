using BaseItems;
using UnityEngine;

namespace Bacteria
{
    public class MoveState : BacteriaState
    {
        private float _motionTime;
        private float _angle;
        private bool _fast;

        public override void BeginState(BacteriaContext context) 
        {
            base.BeginState(context);
            Context.Visual(BacteriaContext.StateName.Move).SetActive(true);
            _motionTime = 0;
        }

        public override void Update()
        {
            base.Update();
            _fast = Context.Target;
            if (Context.TargetInRange)
            {
                Context.SetState(BacteriaContext.StateName.Attack);
                return;
            }

            _motionTime += Time.fixedDeltaTime;
            if (_motionTime >= 1.0f)
            {
                _motionTime -= 1.0f;
                _angle = Context.Target ? TargetAngle() : RandomAngle();
            }

            var dir = Context.Body.rotation.DegreesToVector2();
            Context.Body.velocity =
                dir * (_motionTime.Lerp(0, 1, Context.SpeedBurst, Context.SpeedBurst * Context.SpeedDrop) * (_fast ? 2.2f: 1.0f));
            Context.Body.angularVelocity = _motionTime.Lerp(0, 1, _angle, 0);
        }

        private float RandomAngle()
        {
            var limit = Context.TurnSpeed;
            return Random.Range(-1 * limit, limit);
        }

        private float TargetAngle()
        {
            var currentDir = Context.Body.rotation.NormalizedAngle();
            var targetDir = ((Vector2)Context.Target.transform.position - Context.Body.position).ToDegrees().NormalizedAngle();
            var diff = (targetDir - currentDir).NormalizedAngle();
            return Mathf.Min(Mathf.Abs(diff), Context.TurnSpeed) * Mathf.Sign(diff);
        }

        public override void EndState()
        {
            base.EndState();
            Context.Visual(BacteriaContext.StateName.Move).SetActive(false);
            Context.Body.velocity = Vector2.zero;
            Context.Body.angularVelocity = 0;
        }
    }
}