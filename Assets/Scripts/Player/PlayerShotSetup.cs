using System;
using BaseItems;
using UnityEngine;

namespace Player
{
    public class PlayerShotSetup : BaseBehavior
    {
        [SerializeField] private PlayerShot rootObject;
        [SerializeField] private GameObject baseObject;
        [SerializeField] private GameObject hitObject;
        [SerializeField] private Rigidbody2D body;

        public GameObject BaseObject => baseObject;
        public GameObject HitObject => hitObject;
        public Rigidbody2D Body => body;
        
        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}