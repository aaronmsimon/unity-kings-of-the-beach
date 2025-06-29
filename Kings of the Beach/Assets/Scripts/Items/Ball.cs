using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;
using KotB.Match;
using KotB.StatePattern;
using KotB.StatePattern.MatchStates;
using KotB.StatePattern.BallStates;

namespace KotB.Items
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private BallSO ballInfo;
        [SerializeField] private MatchInfoSO matchInfo;
        [Space(10)]

        [SerializeField] private Transform targetPrefab;
        [SerializeField] private LayerMask obstaclesLayer;
        [SerializeField] private LayerMask invalidAimLayer;
        
        [Header("Game Events")]
        [SerializeField] private GameEvent ballHitGroundEvent;
        [SerializeField] private GameEvent changeToInPlayStateEvent;

        [Header("Variables")]
        [SerializeField] private FloatVariable servePower;

        private Transform ballTarget;

        private StateMachine stateMachine;
        private EventPredicate ballGivenPredicate;
        private EventPredicate targetSetPredicate;
        private EventPredicate ballHitGroundPredicate;
        public EventPredicate deadBallPredicate { get; private set; }

        private void Awake() {
            SetupStateMachine();
        }

        private void Start() {
            ballInfo.BallRadius = GetComponent<SphereCollider>().radius;
        }

        private void OnEnable() {
            ballInfo.TargetSet += OnTargetSet;
        }

        private void OnDisable() {
            ballInfo.TargetSet -= OnTargetSet;

            ballGivenPredicate.Cleanup();
            targetSetPredicate.Cleanup();
            ballHitGroundPredicate.Cleanup();
        }

        private void Update() {
            ballInfo.Position = transform.position;
            if (ballTarget != null) {
                if ((transform.position.x > 0 && ballInfo.Possession == -1) || (transform.position.x < 0 && ballInfo.Possession == 1)) {
                    ballInfo.HitsForTeam = 0;

                    if (matchInfo.CurrentState is ServeState) {
                        changeToInPlayStateEvent.Raise();
                    }
                }
            }
            ballInfo.Possession = (int)Mathf.Sign(transform.position.x);

            stateMachine.Update();
        }

        private void SetupStateMachine() {
            // State Machine
            stateMachine = new StateMachine();

            // Declare States
            var groundState = new GroundState(this);
            var heldState = new HeldState(this);
            var inFlightState = new InFlightState(this);

            // Default Profile
            TransitionProfile defaultProfile = new TransitionProfile("Default");

            // Declare Event Predicates
            ballGivenPredicate = new EventPredicate(stateMachine);
            targetSetPredicate = new EventPredicate(stateMachine);
            ballHitGroundPredicate = new EventPredicate(stateMachine);
            deadBallPredicate = new EventPredicate(stateMachine);

            // Subscribe Event Predicates to Events
            ballInfo.BallGiven += ballGivenPredicate.Trigger;
            ballInfo.TargetSet += targetSetPredicate.Trigger;

            // Define Transitions
            defaultProfile.AddTransition(groundState, heldState, ballGivenPredicate);
            defaultProfile.AddTransition(heldState, inFlightState, targetSetPredicate);
            defaultProfile.AddTransition(inFlightState, groundState, deadBallPredicate);
            defaultProfile.SetStartingState(groundState);

            stateMachine.AddProfile(defaultProfile, true);
        }

        private void OnTargetSet() {
            DestroyBallTarget();
            ballTarget = Instantiate(targetPrefab, ballInfo.TargetPos, Quaternion.identity);
        }

        public void DestroyBallTarget() {
            if (ballTarget != null) {
                Destroy(ballTarget.gameObject);
            }
        }

        //---- PROPERTIES ----
        public BallSO BallInfo { get { return ballInfo; } }
        public StateMachine StateMachine => stateMachine;
        public GameEvent BallHitGround { get { return ballHitGroundEvent; } }
        public LayerMask ObstaclesLayer { get { return obstaclesLayer; } }
        public LayerMask InvalidAimLayer => invalidAimLayer;
    }
}
