using UnityEngine;

namespace Player
{
    public class PlayerContext
    {
        private PlayerState _currentState;
        private PlayerSetup _setup;
        private Player _player;

        public PlayerState CurrentState => _currentState;
        public Vector2 AimInput => _player.AimInput;
        public Vector2 MoveInput => _player.MoveInput;
        public GameObject ElectricStunVisual => _setup.ElectricHitVisual;
        public Rigidbody2D Body => _setup.Body;
        public float TurnSpeed => _player.TurnSpeed;
        public float Speed => _player.Speed;
        public GameObject Turret => _setup.Turret;
        public float TurretTurnSpeed => _player.TurretTurnSpeed;
        public PlayerShot ShotTemplate => _player.ShotTemplate;
        public float ShotsPerSecond => _player.ShotsPerSecond;
        public Transform FirePosition => _setup.FirePosition;
        public Vector2 ApplyImpact => _player.ApplyImpact;

        public PlayerContext(PlayerSetup setup, Player player)
        {
            _setup = setup;
            _player = player;
        }

        public void SetState(StateName state)
        {
            _currentState?.EndState();
            _currentState = StateList[(int)state];
            _currentState.BeginState(this);
        }
        
        public enum StateName
        {
            UserControlled,
            ElectricStun
        }

        private readonly PlayerState[] StateList =
        {
            new UserControlledState(),
            new ElectricStunState()
        };
    }
}