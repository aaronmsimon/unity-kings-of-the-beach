using System;

namespace KotB.StateMachine
{
    public class MatchStateMachine
    {
        public event Action<State> StateChanged;

        private State currentState;

        public void ChangeState(State newState) {
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
