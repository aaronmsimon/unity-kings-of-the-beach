using UnityEngine;
using KotB.StatePattern;
using KotB.Actors;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "Match", menuName = "Game/Match SO")]
    public class MatchSO : ScriptableObject
    {
        private State currentState;
        private Athlete server;

        public State CurrentState {
            get {
                return currentState;
            }
            set {
                currentState = value;
            }
        }
        public Athlete Server {
            get {
                return server;
            }
            set {
                server = value;
            }
        }
    }
}
