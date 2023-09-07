using UnityEngine;

namespace Virus
{
    public class StartFireState : VirusState
    {
        private static readonly int Running = Animator.StringToHash("Running");

        private float _endTime;
        
        public override void BeginState(VirusContext context)
        {
            base.BeginState(context);
            Context.BodyVisual.SetActive(true);
            Context.DieVisual.SetActive(false);
            Context.ShotSpinVisual.SetActive(true);
            Context.ShotAnimator.SetBool(Running, true);
            _endTime = Time.fixedTime + 11.0f / 12.0f;
        }

        public override void Update()
        {
            base.Update();
            if(Time.fixedTime >= _endTime)
                Context.SetState(VirusContext.StateName.Fire);
        }
    }
}