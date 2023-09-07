using BaseItems;
using UnityEngine;

namespace Player
{
    public class UserControlledState : PlayerState
    {
        private float _fireTimer;
        
        public override void Update()
        {
            base.Update();
            var motion = Vector2.zero;
            if (!Context.MoveInput.magnitude.NearZero())
            {
                var dir = BaseBehavior.TurnTowards(Context.Body.rotation, Context.MoveInput.ToDegrees(), Context.TurnSpeed * Context.MoveInput.magnitude);
                Context.Body.rotation = dir;
                motion = dir.DegreesToVector2().normalized *
                         (Context.Speed * Context.MoveInput.magnitude);
            }
            Context.Body.velocity = motion;

            if (!Context.AimInput.magnitude.NearZero())
            {
                Context.Turret.transform.rotation = Quaternion.Euler(0, 0, BaseBehavior.TurnTowards(Context.Turret.transform.rotation.eulerAngles.z, Context.AimInput.ToDegrees(),
                    Context.TurretTurnSpeed * Context.AimInput.magnitude));
                FireShot();
            }
        }
        
        private void FireShot()
        {
            if (Time.fixedTime >= _fireTimer)
            {
                PlayerShot.FireShot(Context.ShotTemplate, Context.FirePosition);
                _fireTimer = Time.fixedTime + 1 / Context.ShotsPerSecond;
            }
        }
    }
}