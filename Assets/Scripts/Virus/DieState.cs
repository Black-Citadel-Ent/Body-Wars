using UnityEngine;

namespace Virus
{
    public class DieState : VirusState
    {
        private float _dieTime;
        
        public override void BeginState(VirusContext context)
        {
            base.BeginState(context);
            Context.DieVisual.SetActive(true);
            Context.BodyVisual.SetActive(false);
            Context.ShotSpinVisual.SetActive(false);
            Context.LifeBar.gameObject.SetActive(false);
            _dieTime = Time.fixedTime + 10.0f / 12.0f;
        }

        public override void Update()
        {
            base.Update();
            if(Time.fixedTime >= _dieTime)
                Object.Destroy(Context.RootObject);
        }
    }
}