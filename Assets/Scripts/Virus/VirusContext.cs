using UnityEngine;

namespace Virus
{
    public class VirusContext
    {
        private VirusState _currentState;
        private VirusSetup _setup;
        private Virus _virus;

        public VirusState State => _currentState;

        public GameObject BodyVisual => _setup.BodyVisual;
        public GameObject ShotSpinVisual => _setup.ShotSpinVisual;
        public GameObject DieVisual => _setup.DieVisual;
        public LifeBar.LifeBar LifeBar => _setup.LifeBar;
        public GameObject RootObject => _virus.gameObject;
        public GameObject[] ShotMasks => _setup.Masks;
        public float ShotSpeed => _virus.ShotSpeed;
        public VirusShot ShotTemplate => _virus.ShotTemplate;
        public float Speed => _virus.Speed;
        public float TargetMinDist => _virus.TargetMinDistance;
        public float TargetMaxDist => _virus.TargetMaxDistance;
        public Animator ShotAnimator => _setup.ShotAnimator;
        public VirusSpawner.VirusSpawner Anchor => _virus.Anchor;
        
        public VirusContext(VirusSetup setup, Virus virus)
        {
            _setup = setup;
            _virus = virus;
        }

        public void SetState(StateName state)
        {
            _currentState?.EndState();
            _currentState = StateList[(int)state];
            _currentState.BeginState(this);
        }
        
        public enum StateName
        {
            Anchored,
            Move,
            StartFire,
            Fire,
            Die
        }

        private readonly VirusState[] StateList =
        {
            new AnchoredState(),
            new MoveState(),
            new StartFireState(),
            new FireState(),
            new DieState()
        };
    }
}