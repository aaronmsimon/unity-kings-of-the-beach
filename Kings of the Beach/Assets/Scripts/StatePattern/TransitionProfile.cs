using System;
using System.Collections.Generic;

namespace KotB.StatePattern
{
    public class TransitionProfile
    {
        private IState startingState;
        private StateNode currentNode;
        private Dictionary<Type, StateNode> nodes = new();
        private HashSet<ITransition> anyTransitions = new();

        public void AddTransition(IState from, IState to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition) {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        public ITransition GetTransition() {
            foreach (var transition in anyTransitions) {
                if (transition.Condition.Evaluate()) {
                    return transition;
                }
            }

            foreach (var transition in currentNode.Transitions) {
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

        public void ChangeState(IState newState) {
            if (newState == currentNode.State) return;

            currentNode.State?.Exit();
            currentNode = nodes[newState.GetType()];
            currentNode.State?.Enter();
        }

        public void SetState(IState newState) {
            currentNode = nodes[newState.GetType()];
            currentNode.State?.Enter();
        }

        public void SetStartingState(IState startState) {
            startingState = startState;
        }

        public void ActivateProfile() {
            SetState(startingState);
        }

        public void Update() {
            currentNode.State?.Update();
        }

        public IState CurrentState { get { return currentNode.State; } }

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