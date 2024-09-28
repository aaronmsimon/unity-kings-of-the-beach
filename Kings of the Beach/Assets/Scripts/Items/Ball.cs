using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;
using KotB.Actors;
using KotB.Match;
using KotB.StatePattern;
using KotB.StatePattern.MatchStates;
using KotB.StatePattern.BallStates;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallSO ballInfo;
    [SerializeField] private MatchInfoSO matchInfo;
    [Space(10)]

    [SerializeField] private Transform targetPrefab;
    
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

    private void Start() {
        ballStateMachine = new StateMachine();
        groundState = new GroundState(this);
        heldState = new HeldState(this);
        inFlightState = new InFlightState(this);
        
        ballStateMachine.ChangeState(groundState);
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
                ballInfo.lastPlayerToHit = null;

                if (matchInfo.CurrentState is ServeState) {
                    changeToInPlayStateEvent.Raise();
                }

                ballInfo.BallChangePossessionEvent();
            }
        }
        ballInfo.Possession = (int)Mathf.Sign(transform.position.x);

        ballStateMachine.Update();

        /* temp */
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) {
            ResetBall();
        }
        #endif
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

    private void ResetBall() {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.StateMachine.ChangeState(player.ServeState);
        ballInfo.GiveBall(player);
        ballStateMachine.ChangeState(heldState);
    }

    //---- PROPERTIES ----
    public BallSO BallInfo { get { return ballInfo; } }
    public StateMachine StateMachine { get { return ballStateMachine; } }
    public GroundState GroundState { get { return groundState; } }
    public HeldState HeldState { get { return heldState; } }
    public InFlightState InFlightState { get { return inFlightState; } }
    public GameEvent BallHitGround { get { return ballHitGroundEvent; } }
}
