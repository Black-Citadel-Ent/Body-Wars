using UnityEngine;
using UnityEngine.Splines;

public class Blob : MonoBehaviour
{
    #region Inspector Fields
    [Header("Root Objects")]
    [SerializeField] private BlobSetup rootObject;
    [SerializeField] private Transform shotRotator;
    [Header("Shot Setup")]
    [SerializeField] private GameObject[] shotMasks;
    [Header("Physics Objects")]
    [SerializeField] private Rigidbody2D body;
    [Header("Animation Objects")]
    [SerializeField] private Animator fireAnimator;
    [Header("Hit Setup")]
    [SerializeField] private GameObject hitEffectRoot;
    [SerializeField] private Transform hitEffectRotator;
    [SerializeField] private Animator hitEffectAnimator;
    [SerializeField] private LifeBar lifeBar;
    #endregion
        
    #region Internal Private Fields
    private StateContext _context;
    private int _remainingLife;
    private float _hitDirection;
    #endregion

    #region Unity Loop 
    private void Start()
    {
        _context = new StateContext
        {
            Blob = this
        };
        _remainingLife = rootObject.Life;

        _context.StartDefault();
    }

    private void Update()
    {
        _context.CurrentState.Update();
        lifeBar.UpdateLife(rootObject.Life, _remainingLife);
    }

    private void FixedUpdate()
    {
        _context.CurrentState.FixedUpdate();
    }
    #endregion
        
    #region Collision Loop
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("PlayerHit"))
            print("Blob Hit Player");
        else if (other.collider.gameObject.layer == LayerMask.NameToLayer("PlayerShot"))
        {
            _hitDirection = other.GetContact(0).relativeVelocity.ToDegrees();
            _context.CurrentState.Hit();
        }
    }
    #endregion
        
    #region State Support Functions
    private Transform FindOpponent()
    {
        return GameObject.FindWithTag("Player")?.transform;
    }
    #endregion

    private enum StateName { MoveTowardsOpponent, DoNothing, Wait, Fire, ReleaseShots, Hit, Die, Hover }

    private class StateContext
    {
        public State CurrentState;
        public Blob Blob;

        public readonly State[] StateList =
        {
            new MoveTowardsOpponentState(),
            new DoNothingState(),
            new WaitState(),
            new FireState(),
            new ReleaseShotState(),
            new HitState(),
            new DieState(),
            new HoverState()
        };

        public void StartDefault()
        {
            CurrentState =
                StateList[(int)(Blob.rootObject.Spawner ? StateName.Hover : StateName.MoveTowardsOpponent)];
            CurrentState.BeginState(this);
        }

        public static readonly int FiringHash = Animator.StringToHash("Firing");
        public static readonly int InterruptHash = Animator.StringToHash("Interrupt");
        public static readonly int HitHash = Animator.StringToHash("Hit");
    }

    private abstract class State
    {
        protected StateContext Context;
        private Transform _target;

        public virtual void BeginState(StateContext context)
        {
            Context = context;
        }

        public virtual void Update() { }
            
        public virtual void FixedUpdate() { }

        protected virtual void EndState() { }
            
        protected void SetState(StateName name)
        {
            Context.CurrentState.EndState();
            Context.CurrentState = Context.StateList[(int)name];
            Context.CurrentState.BeginState(Context);
        }

        public virtual void Hit()
        {
            Context.Blob._remainingLife -= 1;
            SetState(Context.Blob._remainingLife <= 0 ? StateName.Die : StateName.Hit);
        }

        protected Transform GetTarget()
        {
            if(!_target)
                _target = Context.Blob.FindOpponent();
            return _target;
        }
    }

    private class MoveTowardsOpponentState : State
    {
        private Spline _path;
        private float _time;
        private float _startTime;
            
        public override void BeginState(StateContext context)
        {
            base.BeginState(context);
                
            var randomDist = Random.Range(context.Blob.rootObject.MinDistanceFromOpponent,
                context.Blob.rootObject.MaxDistanceFromOpponent);
            var oppTransform = GetTarget();
            if (!oppTransform)
            {
                SetState(StateName.DoNothing);
                return;
            }
                
            var target = oppTransform.position + (Vector3)(new Vector2().Random() * randomDist);
            var distanceToTarget = (target - context.Blob.rootObject.transform.position).magnitude;
                
            _path = new Spline();
            var start = new BezierKnot(Context.Blob.rootObject.transform.position, Vector3.zero, (Vector3)Vector2.zero.Random() * (distanceToTarget / 2.0f));
            var end = new BezierKnot(target, (Vector3)Vector2.zero.Random() * (distanceToTarget / 2.0f), (Vector3)Vector2.zero.Random() * (distanceToTarget / 2.0f));
            _path.Add(start);
            _path.Add(end);

            var dist = _path.GetLength();
            _time = dist / context.Blob.rootObject.Speed;
            _startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            var currentTime = Time.time - _startTime;
            var pos = Mathf.Clamp01(currentTime.Lerp(0, _time, 0, 1));
            var loc = _path.EvaluatePosition(pos);

            Context.Blob.body.MovePosition(new Vector2(loc.x, loc.y));
            if(currentTime >= _time)
                SetState(StateName.Fire);
        }
    }

    private class WaitState : State
    {
        private float _startTime;

        public override void BeginState(StateContext context)
        {
            base.BeginState(context);
            _startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            if(Time.time - _startTime >= Context.Blob.rootObject.WaitTime)
                SetState(StateName.MoveTowardsOpponent);
        }
    }

    private abstract class ShotRotateState : State
    {
        public override void Update()
        {
            base.Update();
            var current = Context.Blob.shotRotator.rotation.eulerAngles.z;
            current += (Context.Blob.rootObject.ShotRotationSpeed * Time.deltaTime);
            Context.Blob.shotRotator.rotation = Quaternion.Euler(0, 0, current.NormalizedAngle());
        }
            
        public override void Hit()
        {
            Context.Blob.fireAnimator.SetTrigger(StateContext.InterruptHash);
            Context.Blob.fireAnimator.SetBool(StateContext.FiringHash, false);
            base.Hit();
        }
    }

    private class FireState : ShotRotateState
    {
        private float _testDelay;

        public override void BeginState(StateContext context)
        {
            base.BeginState(context);
            Context.Blob.fireAnimator.SetBool(StateContext.FiringHash, true);
            _testDelay = Time.time + 0.1f;
            foreach(var mask in Context.Blob.shotMasks)
                mask.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            if (Time.time < _testDelay) return;
                
            var ani = Context.Blob.fireAnimator;
            if (!ani.GetCurrentAnimatorStateInfo(0).IsTag("Creating Shots"))
                SetState(StateName.ReleaseShots);
        }
    }

    private class ReleaseShotState : ShotRotateState
    {
        private float _initTime;
        private int _shotsLeft;

        public override void BeginState(StateContext context)
        {
            base.BeginState(context);
            _shotsLeft = 4;
            _initTime = Time.time - Context.Blob.rootObject.ShotGap;
        }

        public override void Update()
        {
            base.Update();
            if (Time.time - _initTime >= Context.Blob.rootObject.ShotGap)
            {
                if (!GetTarget())
                {
                    SetState(StateName.DoNothing);
                    return;
                }
                BlobShot.StartShot(Context.Blob.rootObject.transform.position, Context.Blob.rootObject.ShotObject);
                _initTime += Context.Blob.rootObject.ShotGap;
                _shotsLeft -= 1;
                Context.Blob.shotMasks[_shotsLeft].SetActive(true);
            }

            if (_shotsLeft == 0)
                SetState(StateName.Wait);
        }

        protected override void EndState()
        {
            Context.Blob.fireAnimator.SetBool(StateContext.FiringHash, false);
            base.EndState();
        }
    }

    private class HitState : State
    {
        private float _holdTime;
        private float _hitTime;

        public override void BeginState(StateContext context)
        {
            base.BeginState(context);
            _holdTime = Time.time + Context.Blob.rootObject.HitStunTime;
            _hitTime = 0;
            Context.Blob.hitEffectRoot.SetActive(true);
            Context.Blob.hitEffectRotator.rotation = Quaternion.Euler(0, 0, (Context.Blob._hitDirection + 180).NormalizedAngle());
            Context.Blob.hitEffectAnimator.SetTrigger(StateContext.HitHash);
        }

        public override void Update()
        {
            base.Update();

            if(_hitTime >= 0.1 && !Context.Blob.hitEffectAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
                Context.Blob.hitEffectRoot.SetActive(false);
                
            if (_hitTime < Context.Blob.rootObject.HitBounceTime)
            {
                var hit0 = _hitTime.Lerp(0, Context.Blob.rootObject.HitBounceTime,
                    Context.Blob.rootObject.HitBounceSpeed, 0);
                _hitTime = Mathf.Clamp(_hitTime + Time.deltaTime, 0, Context.Blob.rootObject.HitBounceTime);
                var hit1 = _hitTime.Lerp(0, Context.Blob.rootObject.HitBounceTime,
                    Context.Blob.rootObject.HitBounceSpeed, 0);
                var motion = Context.Blob._hitDirection.DegreesToVector2() *
                             ((hit0 + hit1) / 2.0f * Time.deltaTime);
                Context.Blob.body.MovePosition(Context.Blob.body.position + motion);
            }

            if(Time.time >= _holdTime)
                SetState(StateName.MoveTowardsOpponent);
        }
    }

    private class DieState : State
    {
        public override void BeginState(StateContext context)
        {
            base.BeginState(context);
            Instantiate(Context.Blob.rootObject.DeathEffect, Context.Blob.rootObject.transform.position,
                Quaternion.Euler(0, 0, Context.Blob._hitDirection + 180.0f));
            Destroy(Context.Blob.rootObject.gameObject);
        }
    }

    private class HoverState : State
    {
        private BlobSpawnerSetup _spawner;
        private Vector2 _targetPoint;

        public override void BeginState(StateContext context)
        {
            base.BeginState(context);
            _spawner = Context.Blob.rootObject.Spawner;
            if (!TestSpawner()) return;

            Context.Blob.rootObject.transform.position = _spawner.transform.position;
            UpdateTargetPoint();
        }

        public override void Update()
        {
            base.Update();
            if (!_spawner)
            {
                SetState(StateName.MoveTowardsOpponent);
                return;
            }
            _spawner.MarkBlob();

            var motion = _targetPoint - (Vector2)Context.Blob.rootObject.transform.position;
            var dist = motion.magnitude;
            var frameMove = Context.Blob.rootObject.Speed * Time.deltaTime;
            if (dist <= frameMove)
            {
                Context.Blob.rootObject.transform.position = _targetPoint;
                UpdateTargetPoint();
            }
            else
                Context.Blob.rootObject.transform.position += (Vector3)(motion.normalized * frameMove);
        }

        private bool TestSpawner()
        {
            if (_spawner) return true;
            SetState(StateName.MoveTowardsOpponent);
            return false;
        }

        private void UpdateTargetPoint()
        {
            // TODO NEXT Create motions between points
            _targetPoint = (Vector2)_spawner.transform.position + (Vector2.zero.Random() * _spawner.MaxSpawnDistance);
        }
    }

    private class DoNothingState : State
    {
        // Opponent Dead, sit there
    }
}