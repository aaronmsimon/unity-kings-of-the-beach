using System;
using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.MatchStates;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Match
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        [SerializeField] private MatchInfoSO matchInfo;
        [SerializeField] private BallSO ballInfo;
        [SerializeField] private InputReader inputReader;

        [Header("Game Events")]
        [SerializeField] private GameEvent scoreUpdate;

        [Header("Prefabs")]
        [SerializeField] private GameObject aiPrefab;
        [SerializeField] private GameObject playerPrefab;

        public event Action BallHitGround;

        private StateMachine matchStateMachine;
        private PrePointState prePointState;
        private ServeState serveState;
        private InPlayState inPlayState;
        private PostPointState postPointState;
        private MatchEndState matchEndState;
        private MatchStartState matchStartState;

        private void Awake() {
            matchStateMachine = new StateMachine();
            prePointState = new PrePointState(this);
            serveState = new ServeState(this);
            inPlayState = new InPlayState(this);
            postPointState = new PostPointState(this);
            matchEndState = new MatchEndState(this);
            matchStartState = new MatchStartState(this);
            
            InitializeMatch();
            matchStateMachine.ChangeState(matchStartState);
        }

        // Temp until teams can be assigned in the start UI
        private void AssignTeams() {

        }

        private void InitializeMatch() {
            matchInfo.TotalPoints = 0;
            matchInfo.ScoreToWin = 21;

            AssignTeams();
        }

        public void ScoreUpdate() {
            scoreUpdate.Raise();
            matchInfo.TotalPoints += 1;
            CheckGameEnd();
        }

        private void CheckGameEnd() {
            for (int i = 0; i < matchInfo.Teams.Length; i++) {
                if (matchInfo.Teams[i].Score.Value == matchInfo.ScoreToWin) {
                    Debug.Log($"{matchInfo.Teams[i].TeamName.Value} wins!");
                    matchStateMachine.ChangeState(matchEndState);
                }
            }
            matchStateMachine.ChangeState(postPointState);
        }

        private void OnEnable() {
            matchStateMachine.StateChanged += OnStateChanged;
        }

        private void OnDisable() {
            matchStateMachine.StateChanged -= OnStateChanged;
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

        //---- PROPERTIES ----
        public StateMachine StateMachine { get { return matchStateMachine; } }
        public PrePointState PrePointState { get { return prePointState; } }
        public ServeState ServeState { get { return serveState; } }
        public InPlayState InPlayState { get { return inPlayState; } }
        public PostPointState PostPointState { get { return postPointState; } }
        public MatchEndState MatchEndState { get { return matchEndState; } }
        public MatchStartState MatchStartState { get { return matchStartState; } }
        public InputReader InputReader { get { return inputReader; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public GameObject AIPrefab { get { return aiPrefab; } }
        public GameObject PlayerPrefab { get { return playerPrefab; } }
    }
}
