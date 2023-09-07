using System.Collections.Generic;
using UnityEngine;

namespace Virus
{
    public class FireState : VirusState
    {
        private List<GameObject> _masks;
        private float _shotTime;

        public override void BeginState(VirusContext context)
        {
            base.BeginState(context);
            _masks = new List<GameObject>(Context.ShotMasks);
            _shotTime = Time.fixedTime + Context.ShotSpeed;
        }

        public override void Update()
        {
            base.Update();
            if (Time.fixedTime >= _shotTime)
            {
                if (_masks.Count > 0)
                {
                    var item = Random.Range(0, _masks.Count);
                    _masks[item].SetActive(true);
                    VirusShot.FireShot(Context.ShotTemplate, _masks[item].transform);
                    _masks.RemoveAt(item);
                    _shotTime = Time.fixedTime + Context.ShotSpeed;
                }
                else
                {
                    Context.SetState(VirusContext.StateName.Move);
                }
            }
        }

        public override void EndState()
        {
            base.EndState();
            foreach (var obj in Context.ShotMasks)
                obj.SetActive(false);
            Context.ShotSpinVisual.SetActive(false);
        }
    }
}