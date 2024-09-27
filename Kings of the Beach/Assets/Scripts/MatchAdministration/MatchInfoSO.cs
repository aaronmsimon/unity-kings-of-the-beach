using UnityEngine;
using KotB.StatePattern;
using KotB.Actors;
using System;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "MatchInfo", menuName = "Game/Match Info")]
    public class MatchInfoSO : ScriptableObject
    {
        private IState currentState;
        private Athlete server;

        public event Action TransitionToServeState;

        public void TransitionToServeStateEvent() {
            TransitionToServeState?.Invoke();
        }

        public IState CurrentState {
            get { return currentState; }
            set { currentState = value; }
        }
        public Athlete Server {
            get { return server; }
            set { server = value; }
        }
    }
}
