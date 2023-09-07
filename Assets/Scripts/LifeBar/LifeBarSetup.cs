using System;
using BaseItems;
using UnityEngine;

namespace LifeBar
{
    public class LifeBarSetup : BaseBehavior
    {
        [SerializeField] private LifeBar rootObject;
        [SerializeField] private GameObject scaleObject;
        [SerializeField] private GameObject visualObject;

        public GameObject ScaleObject => scaleObject;
        public GameObject VisualObject => visualObject;

        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}