using UnityEngine;
using KotB.StatePattern;
using KotB.Actors;
using System;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "MatchInfo", menuName = "Game/Match Info")]
    public class MatchInfoSO : ScriptableObject
    {
        public Team[] Teams { get; set; }
        public int TotalPoints { get; set; }
        public int ScoreToWin { get; set; }
        public int TeamServeIndex { get; set; }
        public int PlayerServeIndex { get; set; }
        
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
    }
}
