using UnityEngine;

namespace Bacteria
{
    public class BacteriaContext
    {
        public enum StateName { Move }
        
        private Bacteria _rootObject;
        private BacteriaSetup _setup;
        private BacteriaState _currentState;

        public BacteriaState State => _currentState;
        public float SpeedBurst => _rootObject.SpeedBurst;
        public float SpeedDrop => _rootObject.SpeedDrop;
        public Rigidbody2D Body => _setup.Body;
        public float Rotation => _rootObject.transform.rotation.eulerAngles.z;

        public BacteriaContext(Bacteria root, BacteriaSetup setup)
        {
            _rootObject = root;
            _setup = setup;
        }
        
        public void SetState(StateName name)
        {
            _currentState?.EndState();
            _currentState = StateList[(int)name];
            _currentState.BeginState(this);
        }

        private BacteriaState[] StateList =
        {
            new MoveState()
        };
    }
}