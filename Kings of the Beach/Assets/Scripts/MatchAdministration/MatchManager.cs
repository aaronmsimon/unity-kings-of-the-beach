using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.MatchStates;
using KotB.Actors;
using System;
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
            
            matchStateMachine.ChangeState(prePointState);
        }

        private void Start() {

            matchInfo.TeamServeIndex = 0;
            matchInfo.PlayerServeIndex = 0;

            // matchInfo.Teams = teams;

            matchInfo.TotalPoints = 0;
            matchInfo.ScoreToWin = 21;
        }

        private void Update() {
            matchStateMachine.Update();
        }

        private void OnEnable() {
            matchStateMachine.StateChanged += OnStateChanged;
        }

        private void OnDisable() {
            matchStateMachine.StateChanged -= OnStateChanged;
        }

        public Athlete GetTeammate(AI ai) {
            return null;
        }

        public void OnBallHitGround() {
            BallHitGround?.Invoke();
        }

        public int GetTeamIndex(Athlete athlete) {
            return -1;
        }

        public int GetOpponentIndex(int teamIndex) {
            if (teamIndex != -1) {
                return Mathf.Abs(teamIndex - 1);
            } else {
                return -1;
            }
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
        public GameEvent ScoreUpdate { get { return scoreUpdate; } }
    }
}
