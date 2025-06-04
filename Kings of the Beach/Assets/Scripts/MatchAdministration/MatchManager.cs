using System.Collections.Generic;
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

        [Header("Transition Profiles")]
        [SerializeField] private StringVariable playerTransitionProfile;
        [SerializeField] private StringVariable aiTransitionProfile;

        public event Action BallHitGround;
        public event Action PointComplete;
        public event Action PostPointComplete;

        private float totalPoints;
        private float switchSidesPointsDivisor = 3;
        private float switchSidesPoints;
        private bool paused = false;
        private IState stateBeforePause;

        private StateMachine stateMachine;

        private EventPredicate matchInitializedPredicate;
        private EventPredicate matchToServePredicate;
        private EventPredicate ballServedPredicate;
        private EventPredicate pointCompletePredicate;
        private EventPredicate matchPostPointCompletePredicate;
        private EventPredicate pausePredicate;

        private void Awake() {
            SetupStateMachine();
        }

        private void Start() {
            InitializeMatch();
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
            stateMachine.StateChanged += OnStateChanged;
            inputReader.pauseEvent += OnPause;
        }

        private void OnDisable() {
            stateMachine.StateChanged -= OnStateChanged;
            inputReader.pauseEvent -= OnPause;

            matchInitializedPredicate.Cleanup();
            matchToServePredicate.Cleanup();
            ballServedPredicate.Cleanup();
            pointCompletePredicate.Cleanup();
            matchPostPointCompletePredicate.Cleanup();
            pausePredicate.Cleanup();
        }

        private void Update() {
            stateMachine.Update();
        }

        private void SetupStateMachine() {
            // State Machine
            stateMachine = new StateMachine();
            
            // Declare States;
            var prePointState = new PrePointState(this);
            var serveState = new ServeState(this);
            var inPlayState = new InPlayState(this);
            var postPointState = new PostPointState(this);
            var matchEndState = new MatchEndState(this);
            var matchStartState = new MatchStartState(this);
            var pauseState = new PauseState(this);

            // Declare Event Predicates
            matchInitializedPredicate = new EventPredicate(stateMachine);
            matchToServePredicate = new EventPredicate(stateMachine);
            ballServedPredicate = new EventPredicate(stateMachine);
            pointCompletePredicate = new EventPredicate(stateMachine);
            matchPostPointCompletePredicate = new EventPredicate(stateMachine);
            pausePredicate = new EventPredicate(stateMachine);

            // Subscribe Event Predicates to Events
            matchInfo.MatchInitialized += matchInitializedPredicate.Trigger;
            matchInfo.TransitionToServeState += matchToServePredicate.Trigger;
            ballInfo.BallServed += ballServedPredicate.Trigger;
            PointComplete += pointCompletePredicate.Trigger;
            PostPointComplete += matchPostPointCompletePredicate.Trigger;

            // Declare Default Profile & Transitions
            TransitionProfile gameProfile = new TransitionProfile("Game");

                // Setup Transitions
                gameProfile.AddAnyTransition(pauseState, pausePredicate);
                gameProfile.AddTransition(matchStartState, prePointState, matchInitializedPredicate);
                gameProfile.AddTransition(prePointState, serveState, matchToServePredicate);
                gameProfile.AddTransition(serveState, inPlayState, ballServedPredicate);
                gameProfile.AddTransition(inPlayState, postPointState, pointCompletePredicate);
                gameProfile.AddTransition(postPointState, matchEndState, new FuncPredicate(() => CheckGameEnd()));
                gameProfile.AddTransition(postPointState, prePointState, matchPostPointCompletePredicate);
                gameProfile.SetStartingState(matchStartState);

                stateMachine.AddProfile(gameProfile, true);
        }

        private bool CheckGameEnd() {
            List<TeamSO> teams = matchInfo.Teams;
            for (int i = 0; i < teams.Count; i++) {
                if (teams[i].Score >= matchInfo.ScoreToWin && Mathf.Abs(teams[i].Score - teams[1 - i].Score) > 1 && teams[i].Score > teams[1 - i].Score) {
                    Debug.Log($"{teams[i].TeamName.Value} wins!");
                    return true;
                }
            }
            return false;
        }

        public void OnBallHitGround() {
            BallHitGround?.Invoke();
        }

        public void InvokePointComplete() {
            PointComplete?.Invoke();
        }

        public void InvokePostPointComplete() {
            PostPointComplete?.Invoke();
        }

        public void OnPause() {
            paused = !paused;
            matchInfo.TogglePauseEvent(paused);
            if (paused) {
                stateBeforePause = stateMachine.CurrentState;
                pausePredicate.Trigger();
            } else {
                stateMachine.SetState(stateBeforePause);
            }
        }

        private void OnStateChanged(IState newState) {
            matchInfo.CurrentState = newState;
        }

        //---- PROPERTIES ----
        public StateMachine StateMachine { get { return stateMachine; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public InputReader InputReader => inputReader;
        public GameObject AIPrefab { get { return aiPrefab; } }
        public GameObject PlayerPrefab { get { return playerPrefab; } }
        public string PlayerTransitionProfileName => playerTransitionProfile.Value;
        public string AITransitionProfileName => aiTransitionProfile.Value;
        public IState StateBeforePause => stateBeforePause;
        public bool Paused { get => paused; set => paused = value; }
    }
}
