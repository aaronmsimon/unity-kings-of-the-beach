using System;

namespace KotB.StatePattern
{
    public class StateMachine
    {
        public event Action<IState> StateChanged;

        private IState currentState;

        public void ChangeState(IState newState) {
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
