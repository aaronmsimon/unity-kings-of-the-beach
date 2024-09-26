using System;
using UnityEngine;

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

        public virtual void OnTriggerEnter(Collider other) {
            currentState?.OnTriggerEnter(other);
        }

        public virtual void OnTriggerExit(Collider other) {
            currentState?.OnTriggerExit(other);
        }

        public IState CurrentState { get { return currentState; } }
    }
}
