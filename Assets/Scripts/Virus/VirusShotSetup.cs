using BaseItems;
using UnityEngine;

namespace Virus
{
    public class VirusShotSetup : BaseBehavior
    {
        [SerializeField] private VirusShot rootObject;
        [SerializeField] private GameObject shotVisual;
        [SerializeField] private GameObject explodeVisual;
        [SerializeField] private Rigidbody2D body;

        public GameObject ShotVisual => shotVisual;
        public GameObject ExplodeVisual => explodeVisual;
        public Rigidbody2D Body => body;
        
        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}