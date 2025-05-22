using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;
using KotB.Actors;
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

        private StateMachine ballStateMachine;
        private GroundState groundState;
        private HeldState heldState;
        private InFlightState inFlightState;

        private void Awake() {
            ballStateMachine = new StateMachine();
            groundState = new GroundState(this);
            heldState = new HeldState(this);
            inFlightState = new InFlightState(this);

            ballStateMachine.ChangeState(groundState);
        }

        private void Start() {
            ballInfo.BallRadius = GetComponent<SphereCollider>().radius;
        }

        private void OnEnable() {
            ballInfo.TargetSet += OnTargetSet;
        }

        private void OnDisable() {
            ballInfo.TargetSet -= OnTargetSet;
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

            ballStateMachine.Update();
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
        public StateMachine StateMachine { get { return ballStateMachine; } }
        public GroundState GroundState { get { return groundState; } }
        public HeldState HeldState { get { return heldState; } }
        public InFlightState InFlightState { get { return inFlightState; } }
        public GameEvent BallHitGround { get { return ballHitGroundEvent; } }
        public LayerMask ObstaclesLayer { get { return obstaclesLayer; } }
        public LayerMask InvalidAimLayer => invalidAimLayer;
    }
}
