using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.MatchStates;
using KotB.Actors;
using System;

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

        public event Action BallHitGround;

        private StateMachine matchStateMachine;
        private PrePointState prePointState;
        private ServeState serveState;
        private InPlayState inPlayState;
        private PostPointState postPointState;
        private MatchEndState matchEndState;

        private void Start() {
            matchStateMachine = new StateMachine();
            prePointState = new PrePointState(this);
            serveState = new ServeState(this);
            inPlayState = new InPlayState(this);
            postPointState = new PostPointState(this);
            matchEndState = new MatchEndState(this);

            matchStateMachine.StateChanged += OnStateChanged;

            matchStateMachine.ChangeState(prePointState);

            matchInfo.TeamServeIndex = 0;
            matchInfo.PlayerServeIndex = 0;

            matchInfo.Teams = teams;

            for (int i = 0; i < teams.Length; i++) {
                for (int j = 0; j < teams[i].Athletes.Length; j++){
                    if (teams[i].Athletes[j] != null && teams[i].Athletes[j] is AI) {
                        AI ai = (AI)teams[i].Athletes[j];
                        ai.Teammate = GetTeammate(ai);
                    }
                }
                teams[i].SetScore(0);
            }

            matchInfo.TotalPoints = 0;
            matchInfo.ScoreToWin = 21;
        }

        private void Update() {
            matchStateMachine.Update();
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

        public void OnBallHitGround() {
            BallHitGround?.Invoke();
        }

        public int GetTeamIndex(Athlete athlete) {
            for (int i = 0; i < teams.Length; i++) {
                for (int j = 0; j < teams[i].Athletes.Length; j++) {
                    if (teams[i].Athletes[j] == athlete) {
                        return i;
                    }
                }
            }
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
        public InputReader InputReader { get { return inputReader; } }
        public MatchInfoSO MatchInfo { get { return matchInfo; } }
        public BallSO BallInfo { get { return ballInfo; } }
        public Team[] Teams { get { return teams; } }
    }
}
