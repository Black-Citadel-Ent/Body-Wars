using UnityEngine;

namespace Bacteria
{
    public class BacteriaContext
    {
        public enum StateName { Move, Attack, Die }
        
        private Bacteria _rootObject;
        private BacteriaSetup _setup;
        private BacteriaState _currentState;
        private GameObject[] _visualElements;

        public BacteriaState State => _currentState;
        public float SpeedBurst => _rootObject.SpeedBurst;
        public float SpeedDrop => _rootObject.SpeedDrop;
        public float TurnSpeed => _rootObject.TurnSpeed;
        public GameObject Target => _rootObject.Target;
        public Rigidbody2D Body => _setup.Body;
        public LifeBar.LifeBar LifeBar => _setup.LifeBar;
        public GameObject Colliders => _setup.Colliders;
        public bool TargetInRange => _rootObject.TargetInRange;
        public float AttackDamage => _rootObject.AttackDamage;

        public GameObject Visual(StateName name)
        {
            return _visualElements[(int)name];
        }

        public BacteriaContext(Bacteria root, BacteriaSetup setup)
        {
            _rootObject = root;
            _setup = setup;
            _visualElements = new []
            {
                _setup.MoveVisual,
                _setup.AttackVisual,
                _setup.DieVisual
            };
            foreach(var item in _visualElements)
                item.SetActive(false);
        }
        
        public void SetState(StateName name)
        {
            _currentState?.EndState();
            _currentState = StateList[(int)name];
            _currentState.BeginState(this);
        }

        private BacteriaState[] StateList =
        {
            new MoveState(),
            new AttackState(),
            new DieState()
        };
    }
}