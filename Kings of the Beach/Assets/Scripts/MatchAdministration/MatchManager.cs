using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.MatchStates;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;
using KotB.Items;

namespace KotB.Match
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        [SerializeField] private MatchInfoSO matchInfo;
        [SerializeField] private BallSO ballInfo;
        [SerializeField] private FloatVariable serveCamDirection;
        [SerializeField] private InputReader inputReader;

        [Header("Match Variables")]
        [SerializeField] private FloatVariable scoreToWin;

        [Header("Game Events")]
        [SerializeField] private GameEvent scoreUpdate;

        [Header("Prefabs")]
        [SerializeField] private GameObject aiPrefab;
        [SerializeField] private GameObject playerPrefab;

        public event Action BallHitGround;

        private float totalPoints;
        private float switchSidesPointsDivisor = 3;
        private float switchSidesPoints;
        private bool paused = false;
        private IState stateBeforePause;

        private StateMachine matchStateMachine;
        private PrePointState prePointState;
        private ServeState serveState;
        private InPlayState inPlayState;
        private PostPointState postPointState;
        private MatchEndState matchEndState;
        private MatchStartState matchStartState;
        private PauseState pauseState;

        private void Awake() {
            matchStateMachine = new StateMachine();
            prePointState = new PrePointState(this);
            serveState = new ServeState(this);
            inPlayState = new InPlayState(this);
            postPointState = new PostPointState(this);
            matchEndState = new MatchEndState(this);
            matchStartState = new MatchStartState(this);
            pauseState = new PauseState(this);
        }

        private void Start() {
            InitializeMatch();
            matchStateMachine.ChangeState(matchStartState);
        }

        private void InitializeMatch() {
            totalPoints = 0;
            scoreToWin.Value = 21;
        }

        public void ScoreUpdate() {
            scoreUpdate.Raise();
            totalPoints += 1;
            switchSidesPoints = scoreToWin.Value / switchSidesPointsDivisor;
            if (totalPoints % switchSidesPoints == 0) {
                matchInfo.TeamsSwitchSides();
                serveCamDirection.Value *= -1;
            }
        }

        private void OnEnable() {
            matchStateMachine.StateChanged += OnStateChanged;
            inputReader.pauseEvent += OnPause;
        }

        private void OnDisable() {
            matchStateMachine.StateChanged -= OnStateChanged;
            inputReader.pauseEvent -= OnPause;
        }

        private void Update() {
            matchStateMachine.Update();
        }

        public void OnBallHitGround() {
            BallHitGround?.Invoke();
        }

        private void OnStateChanged(IState newState) {
            matchInfo.CurrentState = newState;
        }

        private void OnPause() {
            if (!paused) {
                stateBeforePause = matchStateMachine.CurrentState;
                matchStateMachine.ChangeState(pauseState);
            }
            paused = !paused;
            matchInfo.TogglePauseEvent(paused);
        }

        //---- PROPERTIES ----
        public StateMachine StateMachine { get { return matchStateMachine; } }
        public PrePointState PrePointState { get { return prePointState; } }
        public ServeState ServeState { get { return serveState; } }
        public InPlayState InPlayState { get { return inPlayState; } }
        public PostPointState PostPointState { get { return postPointState; } }
        public MatchEndState MatchEndState { get { return matchEndState; } }
        public MatchStartState MatchStartState { get { return matchStartState; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public InputReader InputReader => inputReader;
        public GameObject AIPrefab { get { return aiPrefab; } }
        public GameObject PlayerPrefab { get { return playerPrefab; } }
        public IState StateBeforePause => stateBeforePause;
        public bool Paused { get => paused; set => paused = value; }
    }
}
