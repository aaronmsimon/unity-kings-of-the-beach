using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.MatchStates;
using RoboRyanTron.Unite2017.Events;
using KotB.Actors;

namespace KotB.Match
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Teams")]
        [SerializeField] private Team[] teams = new Team[2];

        [Header("Scriptable Objects")]
        [SerializeField] private MatchInfoSO matchInfo;
        [SerializeField] private BallSO ballInfo;
        [SerializeField] private InputReader inputReader;

        private int teamServeIndex = 0;
        private int playerServeIndex = 0;

        private StateMachine matchStateMachine;
        private PrePointState prePointState;
        private ServeState serveState;
        private InPlayState inPlayState;
        private PostPointState postPointState;

        private void Start() {
            matchStateMachine = new StateMachine();
            prePointState = new PrePointState(this);
            serveState = new ServeState(this);
            inPlayState = new InPlayState(this);
            postPointState = new PostPointState(this);

            matchStateMachine.StateChanged += OnStateChanged;

            matchStateMachine.ChangeState(prePointState);
            UpdateMatchSOServer();

            for (int i = 0; i < teams.Length; i++) {
                for (int j = 0; j < teams[i].Athletes.Length; j++){
                    if (teams[i].Athletes[j] != null && teams[i].Athletes[j] is AI) {
                        AI ai = (AI)teams[i].Athletes[j];
                        ai.Teammate = GetTeammate(ai);
                    }
                }
            }
        }

        public Athlete GetTeammate(AI ai) {
            for (int i = 0; i < teams.Length; i++) {
                for (int j = 0; j < teams[i].Athletes.Length; j++) {
                    if (teams[i].Athletes[j] == ai) {
                        return teams[i].Athletes[Mathf.Abs(j - 1)];
                    }
                }
            }

            return null;
        }

        public void NextServer() {
            teamServeIndex = Mathf.Abs(teamServeIndex - 1);
            if (teamServeIndex == 0)
                playerServeIndex = Mathf.Abs(playerServeIndex - 1);
            
            UpdateMatchSOServer();
        }

        public void OnBallHitGround() {
            matchStateMachine.ChangeState(postPointState);
        }

        private void UpdateMatchSOServer() {
            matchInfo.Server = teams[teamServeIndex].Athletes[playerServeIndex];
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
        public InputReader InputReader { get { return inputReader; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public BallSO BallInfo { get { return ballInfo; } }
    }
}
