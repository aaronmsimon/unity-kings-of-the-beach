using UnityEngine;
using KotB.Actors;
using KotB.StatePattern;
using KotB.Match.MatchStates;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Match
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Teams")]
        [SerializeField] private Team[] teams = new Team[2];

        [Header("Configuration")]
        [SerializeField] private MatchSO matchSO;
        [SerializeField] private InputReader inputReader;

        [Header("Events")]
        [SerializeField] private GameEvent changeToServeState;
        [SerializeField] private GameEvent showPowerMeter;

        private int teamServeIndex = 0;
        private int playerServeIndex = 0;

        private StateMachine matchStateMachine;
        private PrePointState prePointState;
        private ServeState serveState;

        private void Start() {
            matchStateMachine = new StateMachine();
            prePointState = new PrePointState(inputReader, changeToServeState);
            serveState = new ServeState(inputReader);

            matchStateMachine.StateChanged += OnStateChanged;

            matchStateMachine.ChangeState(prePointState);
        }

        public void OnChangeToServeState() {
            matchStateMachine.ChangeState(serveState);
        }

        public void NextServer() {
            teamServeIndex = Mathf.Abs(teamServeIndex - 1);
            if (teamServeIndex == 0)
                playerServeIndex = Mathf.Abs(playerServeIndex - 1);
            
            matchSO.Server = teams[teamServeIndex].Athletes[playerServeIndex];
        }

        private void OnStateChanged(State newState) {
            matchSO.CurrentState = newState;
        }

        // Class for Match
        [System.Serializable]
        private class Team
        {
            [SerializeField] private string teamName;
            [SerializeField] private Athlete[] athletes = new Athlete[2];

            public Athlete[] Athletes {
                get {
                    return athletes;
                }
            }
        }
    }
}
