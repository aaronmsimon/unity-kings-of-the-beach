using UnityEngine;
using KotB.StatePattern;
using KotB.Actors;
using System;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "MatchInfo", menuName = "Game/Match Info")]
    public class MatchInfoSO : ScriptableObject
    {
        [SerializeField] private Team[] teams;
        public int TotalPoints { get; set; }
        public int ScoreToWin { get; set; }
        [SerializeField] private FloatVariable teamServeIndex;
        [SerializeField] private FloatVariable playerServeIndex;

        private IState currentState;

        public event Action TransitionToPrePointState;
        public event Action TransitionToServeState;

        public void TransitionToPrePointStateEvent() {
            TransitionToPrePointState?.Invoke();
        }
        
        public void TransitionToServeStateEvent() {
            TransitionToServeState?.Invoke();
        }

        public IState CurrentState {
            get { return currentState; }
            set { currentState = value; }
        }
        public Athlete Server {
            get { return Teams[TeamServeIndex].Athletes[PlayerServeIndex]; }
        }

        //---- PROPERTIES ----
        public int TeamServeIndex { get { return (int)teamServeIndex.Value; } set { teamServeIndex.Value = value; } }
        public int PlayerServeIndex { get { return (int)playerServeIndex.Value; } set { playerServeIndex.Value = value; } }
        public Team[] Teams { get { return teams; } set { teams = value; } }
    }
}
