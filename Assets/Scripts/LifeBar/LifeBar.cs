using BaseItems;
using UnityEngine;

namespace LifeBar
{
    public class LifeBar: BaseBehavior
    {
        private LifeBarSetup _setup;

        public void RegisterSetup(LifeBarSetup setup)
        {
            _setup = setup;
        }

        public void SetLife(float value)
        {
            value = Mathf.Clamp01(value);
            _setup.ScaleObject.transform.localScale = new Vector3(value, 1, 1);
            _setup.VisualObject.SetActive(!(value - 1.0f).NearZero());
        }
    }
}