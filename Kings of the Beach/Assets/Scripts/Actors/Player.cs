using UnityEngine;
using KotB.StatePattern.PlayerStates;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;
using KotB.StatePattern;

namespace KotB.Actors
{
    public class Player : Athlete
    {
        [Header("Player Controls")]
        [SerializeField] private float coyoteTime;
        [SerializeField] private InputReader inputReader;

        [Header("Game Events")]
        [SerializeField] private GameEvent updateCameraPriority;
        [SerializeField] private GameEvent showServePowerMeter;
        [SerializeField] private GameEvent hideServePowerMeter;
        [SerializeField] private GameEvent showServeAim;
        [SerializeField] private GameEvent hideServeAim;
        [SerializeField] private GameEvent pauseGame;
        [SerializeField] private GameEvent unpauseGame;
        
        [Header("Variables")]
        [SerializeField] private FloatVariable servePowerValue;
        [SerializeField] private FloatVariable mainCameraPriority;
        [SerializeField] private FloatVariable serveCameraPriority;
        [SerializeField] private Vector3Variable serveAimPosition;


        private Vector3 moveInput;
        private Vector3 rightStickInput;
        private bool paused;
        private IState stateBeforePause;

        private NormalState normalState;
        private LockState lockState;
        private ServeState serveState;
        private PostPointState postPointState;
        private PauseState pauseState;

        protected override void Awake() {
            base.Awake();

            normalState = new NormalState(this);
            lockState = new LockState(this);
            serveState = new ServeState(this);
            postPointState = new PostPointState(this);
            pauseState = new PauseState(this);
            
            stateMachine.ChangeState(postPointState);
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable() {
            inputReader.moveEvent += OnMove;
            inputReader.rightStickEvent += OnRightStick;
            inputReader.jumpEvent += OnJump;
            inputReader.feintEvent += OnJumpModified;
            inputReader.pauseEvent += OnPause;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable() {
            inputReader.moveEvent -= OnMove;
            inputReader.rightStickEvent -= OnRightStick;
            inputReader.jumpEvent -= OnJump;
            inputReader.feintEvent -= OnJumpModified;
            inputReader.pauseEvent -= OnPause;
        }

        //---- EVENT LISTENERS ----

        public void OnBallHitGround() {
            moveDir = Vector3.zero;
            stateMachine.ChangeState(postPointState);
        }

        private void OnMove(Vector2 movement) {
            moveInput = movement;
        }

        private void OnRightStick(Vector2 movement) {
            rightStickInput = movement;
        }

        private void OnJump() {
            feint = false;
            PerformJump();
        }

        private void OnJumpModified() {
            feint = true;
            PerformJump();
        }

        private void OnPause() {
            if (!paused) {
                paused = true;
                stateBeforePause = stateMachine.CurrentState;
                stateMachine.ChangeState(pauseState);
                pauseGame.Raise();
            } else {
                paused = false;
                stateMachine.ChangeState(stateBeforePause);
                unpauseGame.Raise();
            }
        }

        //---- PROPERTIES ----

        public InputReader InputReader { get { return inputReader; } }
        public NormalState NormalState { get { return normalState; } }
        public LockState LockState { get { return lockState; } }
        public ServeState ServeState { get { return serveState; } }
        public PostPointState PostPointState { get { return postPointState; } }
        public Vector3 MoveInput { get { return moveInput; } }
        public Vector3 RightStickInput { get { return rightStickInput; } }
        public FloatVariable ServePowerValue {
            get { return servePowerValue; }
            set { servePowerValue = value; }
        }
        public GameEvent UpdateCameraPriorty { get { return updateCameraPriority; } }
        public GameEvent ShowServePowerMeter { get { return showServePowerMeter; } }
        public GameEvent HideServePowerMeter { get { return hideServePowerMeter; } }
        public GameEvent ShowServeAim { get { return showServeAim; } }
        public GameEvent HideServeAim { get { return hideServeAim; } }
        public GameEvent PauseGame => pauseGame;
        public GameEvent UnpauseGame => unpauseGame;
        public FloatVariable MainCameraPriority {
            get { return mainCameraPriority; }
            set { mainCameraPriority = value; }
        }
        public FloatVariable ServeCameraPriority {
            get { return serveCameraPriority; }
            set { serveCameraPriority = value; }
        }
        public Vector3Variable ServeAimPosition {
            get { return serveAimPosition; }
            set {
                serveAimPosition = value;
            }
        }
        public float CoyoteTime { get { return coyoteTime; } }
    }
}
