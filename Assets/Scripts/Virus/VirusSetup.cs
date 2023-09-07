using BaseItems;
using UnityEngine;

namespace Virus
{
    public class VirusSetup : BaseBehavior
    {
        [SerializeField] private Virus rootObject;
        [SerializeField] private GameObject bodyVisual;
        [SerializeField] private GameObject shotSpinVisual;
        [SerializeField] private GameObject dieVisual;
        [SerializeField] private Animator shotAnimator;
        [SerializeField] private GameObject[] masks;
        [SerializeField] private LifeBar.LifeBar lifeBar;

        public GameObject BodyVisual => bodyVisual;
        public GameObject ShotSpinVisual => shotSpinVisual;
        public GameObject DieVisual => dieVisual;
        public Animator ShotAnimator => shotAnimator;
        public GameObject[] Masks => masks;
        public LifeBar.LifeBar LifeBar => lifeBar;

        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}