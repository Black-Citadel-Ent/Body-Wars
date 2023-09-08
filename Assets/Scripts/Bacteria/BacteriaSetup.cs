using System;
using BaseItems;
using UnityEngine;

namespace Bacteria
{
    public class BacteriaSetup : BaseBehavior
    {
        [SerializeField] private Bacteria rootObject;
        [SerializeField] private Rigidbody2D body;

        public Rigidbody2D Body => body;
        
        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}