using BaseItems;
using UnityEngine;

namespace Player
{
    public class PlayerSetup : BaseBehavior
    {
        [SerializeField] private Player rootObject;
        [SerializeField] private GameObject turret;
        [SerializeField] private Transform firePosition;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private LifeBar.LifeBar lifeBar;
        [SerializeField] private GameObject electricHitVisual;

        public GameObject Turret => turret;
        public Transform FirePosition => firePosition;
        public Rigidbody2D Body => body;
        public LifeBar.LifeBar LifeBar => lifeBar;
        public GameObject ElectricHitVisual => electricHitVisual;

        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}