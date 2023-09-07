using BaseItems;
using UnityEngine;
using UnityEngine.Splines;

namespace Virus
{
    public class AnchoredState : VirusState
    {
        private Spline _path;
        private float _pos;
        private float _length;
        private float _updatePos;
        
        public override void BeginState(VirusContext context)
        {
            base.BeginState(context);
            Context.DieVisual.SetActive(false);
            Context.BodyVisual.SetActive(true);
            Context.ShotSpinVisual.SetActive(false);

            _path = new Spline(new BezierKnot[]
            {
                new(Context.Anchor.transform.position),
                new(CreatePoint()),
                new(CreatePoint())
            });
            _path.SetTangentMode(TangentMode.AutoSmooth);
            _length = _path.GetLength();
            _updatePos = _path.GetCurveLength(0) / _length;
            _pos = 0.0f;
        }

        public override void Damage()
        {
            base.Damage();
            Context.Anchor.Detach(Context.RootObject);
            Context.SetState(VirusContext.StateName.Move);
        }

        public override void Update()
        {
            base.Update();
            _pos += Context.Speed * 0.5f / _length * Time.fixedDeltaTime;
            Context.RootObject.transform.position = _path.EvaluatePosition(_pos);
            if(_pos > _updatePos)
                UpdatePath();
        }

        private Vector3 CreatePoint()
        {
            var dir = Random.Range(-180.0f, 180.0f);
            var dist = Random.Range(0.0f, Context.Anchor.AnchorRadius);
            return (Vector2)Context.Anchor.transform.position + dir.DegreesToVector2() * dist;
        }

        private void UpdatePath()
        {
            var oldSpanDist = _path.GetCurveLength(0);
            _path.RemoveAt(0);
            _path.Add(new BezierKnot(CreatePoint()));
            var newLength = _path.GetLength();

            var origDist = _pos * _length;
            var currentDist = origDist - oldSpanDist;
            
            _pos = currentDist / newLength;
            _length = newLength;
            _updatePos = _path.GetCurveLength(0) / _length;
        }
    }
}