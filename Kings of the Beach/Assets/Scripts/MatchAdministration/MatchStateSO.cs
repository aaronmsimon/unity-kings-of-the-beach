using UnityEngine;

namespace KotB.StateMachine
{
    [CreateAssetMenu(fileName = "MatchState", menuName = "Game/Match State")]
    public class MatchStateSO : ScriptableObject
    {
        private State currentState;

        public State CurrentState {
            get {
                return currentState;
            }
            set {
                currentState = value;
            }
        }
    }
}
