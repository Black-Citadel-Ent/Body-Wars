using BaseItems;
using UnityEngine;

namespace VirusSpawner
{
    public class VirusSpawnerSetup : BaseBehavior
    {
        [SerializeField] private VirusSpawner rootObject;
        [SerializeField] private GameObject smallVisual;
        [SerializeField] private GameObject mediumVisual;
        [SerializeField] private GameObject largeVisual;
        [SerializeField] private GameObject smallToDeadVisual;
        [SerializeField] private GameObject mediumToSmallVisual;
        [SerializeField] private GameObject largeToMediumVisual;
        [SerializeField] private LifeBar.LifeBar lifeBar;

        public GameObject SmallVisual => smallVisual;
        public GameObject MediumVisual => mediumVisual;
        public GameObject LargeVisual => largeVisual;
        public GameObject SmallToDeadVisual => smallToDeadVisual;
        public GameObject MediumToSmallVisual => mediumToSmallVisual;
        public GameObject LargeToMediumVisual => largeToMediumVisual;
        public LifeBar.LifeBar LifeBar => lifeBar;
        
        private void Awake()
        {
            rootObject.RegisterSetup(this);
        }
    }
}