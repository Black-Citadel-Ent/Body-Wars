using BaseItems;
using UnityEngine;
using UnityEngine.Splines;

namespace Virus
{
    public class MoveState : VirusState
    {
        private Spline _motionPath;
        private float _startTime;
        private float _endTime;
        
        public override void BeginState(VirusContext context)
        {
            base.BeginState(context);
            Context.BodyVisual.SetActive(true);
            Context.DieVisual.SetActive(false);
            Context.ShotSpinVisual.SetActive(false);

            var initial = Context.RootObject.transform.position;
            var target = CreateTargetPosition();
            var intermediate = CreateIntermediatePosition(initial, target);

            _motionPath = new Spline(new BezierKnot[]
            {
                new(initial),
                new(intermediate),
                new(target)
            });
            _motionPath.SetTangentMode(TangentMode.AutoSmooth);

            _startTime = Time.fixedTime;
            _endTime = Time.fixedTime + _motionPath.GetLength() / Context.Speed;
        }

        public override void Update()
        {
            base.Update();
            var pos = Time.fixedTime.ClampLerp(_startTime, _endTime, 0, 1);
            Context.RootObject.transform.position = _motionPath.EvaluatePosition(pos);
            if((pos - 1.0f).NearZero())
                Context.SetState(VirusContext.StateName.StartFire);
        }

        private Vector2 FindPlayerPosition()
        {
            return GameObject.FindWithTag("Player").transform.position;
        }

        private Vector3 CreateTargetPosition()
        {
            var center = FindPlayerPosition();
            var dist = Random.Range(Context.TargetMinDist, Context.TargetMaxDist);
            var dir = BaseBehavior.RandomVector2();
            return center + dir * dist;
        }

        private Vector3 CreateIntermediatePosition(Vector3 initial, Vector3 target)
        {
            var motion = target - initial;
            var randAngle = Random.Range(-30.0f, 30.0f) + ((Vector2)motion).ToDegrees();
            var dist = motion.magnitude / 2.0f;
            return initial + (Vector3)randAngle.DegreesToVector2() * dist;
        }
    }
}