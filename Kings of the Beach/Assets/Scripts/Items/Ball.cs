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

            ballInfo.BallGiven -= ballGivenPredicate.Trigger;
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

            // Declare Event Predicates
            ballGivenPredicate = new EventPredicate();
            targetSetPredicate = new EventPredicate();
            ballHitGroundPredicate = new EventPredicate();

            // Subscribe Event Predicates to Events
            ballInfo.BallGiven += ballGivenPredicate.Trigger;

            // Declare Default Profile
            TransitionProfile defaultProfile = new TransitionProfile();

            // Define Transitions
            defaultProfile.AddAnyTransition(groundState, ballHitGroundPredicate);
            defaultProfile.AddTransition(groundState, heldState, ballGivenPredicate);
            defaultProfile.AddTransition(heldState, inFlightState, targetSetPredicate);
            defaultProfile.SetStartingState(groundState);

            stateMachine.AddProfile(defaultProfile, true);
        }

        private void OnTargetSet() {
            DestroyBallTarget();
            ballTarget = Instantiate(targetPrefab, ballInfo.TargetPos, Quaternion.identity);

            if (stateMachine.CurrentProfile.CurrentState is HeldState) {
                targetSetPredicate.Trigger();
            }
        }

        public void OnBallHitGround() {
            ballHitGroundPredicate.Trigger();
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
