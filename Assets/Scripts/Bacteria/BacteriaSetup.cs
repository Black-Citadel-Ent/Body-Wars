using System;
using BaseItems;
using UnityEngine;

namespace Bacteria
{
    public class BacteriaSetup : BaseBehavior
    {
        [SerializeField] private Bacteria rootObject;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private LifeBar.LifeBar lifeBar;
        [SerializeField] private GameObject moveVisual;
        [SerializeField] private GameObject attackVisual;
        [SerializeField] private GameObject dieVisual;
        [SerializeField] private GameObject colliders;

        public Rigidbody2D Body => body;
        public LifeBar.LifeBar LifeBar => lifeBar;
        public GameObject MoveVisual => moveVisual;
        public GameObject AttackVisual => attackVisual;
        public GameObject DieVisual => dieVisual;
        public GameObject Colliders => colliders;
        
        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}