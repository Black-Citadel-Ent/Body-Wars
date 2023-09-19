using UnityEngine;

namespace Bacteria
{
    public class DieState : BacteriaState
    {
        private float _endTime;
        
        public override void BeginState(BacteriaContext context)
        {
            base.BeginState(context);
            Context.Visual(BacteriaContext.StateName.Die).SetActive(true);
            Context.LifeBar.gameObject.SetActive(false);
            _endTime = Time.fixedTime + 5.0f / 12.0f;
            Context.Colliders.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            if(Time.fixedTime >= _endTime)
                Object.Destroy(Context.Body.gameObject);
        }
    }
}