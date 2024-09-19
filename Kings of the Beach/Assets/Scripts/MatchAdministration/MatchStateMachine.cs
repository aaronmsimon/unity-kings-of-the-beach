using System;

namespace KotB.StateMachine
{
    public class MatchStateMachine
    {
        public event Action<State> StateChanged;

        private MatchState currentState;

        public void ChangeState(MatchState newState) {
            currentState?.Exit();

            currentState = newState;
            StateChanged?.Invoke(newState);
            currentState?.Enter();
        }

        public void Update() {
            currentState?.Update();
        }
    }
}
