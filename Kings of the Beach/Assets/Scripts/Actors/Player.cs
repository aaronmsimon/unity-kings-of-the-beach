using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.PlayerStates;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;

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
        
        [Header("Variables")]
        [SerializeField] private FloatVariable servePowerValue;
        [SerializeField] private FloatVariable mainCameraPriority;
        [SerializeField] private FloatVariable serveCameraPriority;
        [SerializeField] private Vector3Variable serveAimPosition;

        private Vector3 moveInput;
        private Vector3 rightStickInput;

        private event Action lockPlayer;
        private event Action unlockPlayer;

        private EventPredicate ballHitGroundPredicate;
        private EventPredicate matchToPrePointPredicate;
        private EventPredicate matchToServePredicate;
        private EventPredicate serveTargetSetPredicate;
        private EventPredicate lockPlayerPredicate;
        private EventPredicate unlockPlayerPredicate;

        protected override void Awake() {
            base.Awake();
            
            SetupStateMachine();
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable() {
            inputReader.moveEvent += OnMove;
            inputReader.rightStickEvent += OnRightStick;
            inputReader.jumpEvent += OnJump;
            inputReader.feintEvent += OnJumpModified;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable() {
            inputReader.moveEvent -= OnMove;
            inputReader.rightStickEvent -= OnRightStick;
            inputReader.jumpEvent -= OnJump;
            inputReader.feintEvent -= OnJumpModified;

            matchToPrePointPredicate.Cleanup();
            matchToServePredicate.Cleanup();
            serveTargetSetPredicate.Cleanup();
            lockPlayerPredicate.Cleanup();
            unlockPlayerPredicate.Cleanup();
        }

        private void SetupStateMachine() {
            // State Machine
            stateMachine = new StateMachine();
            
            // Declare States
            var normalState = new NormalState(this);
            var lockState = new LockState(this);
            var serveState = new ServeState(this);
            var postPointState = new PostPointState(this);

            // Declare Event Predicates
            ballHitGroundPredicate = new EventPredicate(stateMachine);

            matchToPrePointPredicate = new EventPredicate(stateMachine);
            matchToServePredicate = new EventPredicate(stateMachine);
            serveTargetSetPredicate = new EventPredicate(stateMachine);
            lockPlayerPredicate = new EventPredicate(stateMachine);
            unlockPlayerPredicate = new EventPredicate(stateMachine);

            // Subscribe Event Predicates to Events
            matchInfo.TransitionToPrePointState += matchToPrePointPredicate.Trigger;
            matchInfo.TransitionToServeState += matchToServePredicate.Trigger;
            ballInfo.BallServed += serveTargetSetPredicate.Trigger;
            lockPlayer += lockPlayerPredicate.Trigger;
            unlockPlayer += unlockPlayerPredicate.Trigger;

            // Declare Default Profile & Transitions
            TransitionProfile gameProfile = new TransitionProfile("Game");

                // Setup Transitions
                gameProfile.AddAnyTransition(postPointState, ballHitGroundPredicate);
                gameProfile.AddTransition(postPointState, normalState, matchToPrePointPredicate);
                gameProfile.AddTransition(normalState, serveState, new AndPredicate(
                    matchToServePredicate,
                    new FuncPredicate(() => matchInfo.GetServer() == this)
                ));
                gameProfile.AddTransition(normalState, lockState, lockPlayerPredicate);
                gameProfile.AddTransition(lockState, normalState, unlockPlayerPredicate);
                gameProfile.AddTransition(serveState, normalState, serveTargetSetPredicate);
                gameProfile.SetStartingState(postPointState);

                stateMachine.AddProfile(gameProfile);

            // Declare Tutorial Profile & Transitions
            TransitionProfile tutorialProfile = new TransitionProfile("Tutorial");

                // Setup Transitions
                tutorialProfile.AddAnyTransition(normalState, ballHitGroundPredicate);
                tutorialProfile.AddTransition(normalState, lockState, lockPlayerPredicate);
                tutorialProfile.AddTransition(lockState, normalState, unlockPlayerPredicate);
                // tutorialProfile.AddTransition(normalState, serveState, new AndPredicate(
                //     matchToServePredicate,
                //     new FuncPredicate(() => matchInfo.GetServer() == this)
                // ));
                // tutorialProfile.AddTransition(serveState, normalState, serveTargetSetPredicate);
                tutorialProfile.SetStartingState(normalState);

                stateMachine.AddProfile(tutorialProfile);
        }

        public void LockPlayer() {
            lockPlayer?.Invoke();
        }

        public void UnlockPlayer() {
            unlockPlayer?.Invoke();
        }

        //---- EVENT LISTENERS ----

        public void OnBallHitGround() {
            ballHitGroundPredicate.Trigger();
            moveDir = Vector3.zero;
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

        //---- PROPERTIES ----

        public InputReader InputReader { get { return inputReader; } }
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
