using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KotB.StatePattern
{
    public class StateMachine
    {
        private StateNode current;
        private Dictionary<Type, StateNode> nodes = new();
        private HashSet<ITransition> anyTransitions = new();

        public event Action<IState> StateChanged;

        public void ChangeState(IState newState) {
            if (newState == current.State) return;

            current.State?.Exit();

            current = nodes[newState.GetType()];
            StateChanged?.Invoke(newState);
            current.State?.Enter();
        }

        public void Update() {
            var transition = GetTransition();
            if (transition != null) {
                ChangeState(transition.To);
            }
            current.State?.Update();
        }

        // public virtual void OnTriggerEnter(Collider other) {
        //     currentState?.OnTriggerEnter(other);
        // }

        // public virtual void OnTriggerExit(Collider other) {
        //     currentState?.OnTriggerExit(other);
        // }

        public void SetState(IState newState) {
            current = nodes[newState.GetType()];
            current.State?.Enter();
        }

        public void AddTransition(IState from, IState to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition) {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }


        private ITransition GetTransition() {
            foreach (var transition in anyTransitions) {
                if (transition.Condition.Evaluate()) {
                    return transition;
                }
            }

            foreach (var transition in current.Transitions) {
                if (transition.Condition.Evaluate()) {
                    return transition;
                }
            }

            return null;
        }

        private StateNode GetOrAddNode(IState state) {
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null) {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        public IState CurrentState { get { return current.State; } }

        private class StateNode {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state) {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition) {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}
