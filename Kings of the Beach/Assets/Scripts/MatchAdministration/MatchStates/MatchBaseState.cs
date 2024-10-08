using KotB.Match;
using UnityEngine;

namespace KotB.StatePattern.MatchStates
{
    public abstract class MatchBaseState : IState
    {
        protected readonly MatchManager matchManager;

        protected MatchBaseState(MatchManager context) {
            matchManager = context;
        }

        public virtual void Enter() {
            // no op
        }

        public virtual void Exit() {
            // no op
        }

        public virtual void Update() {
            // no op
        }

        public virtual void OnTriggerEnter(Collider other) {
            // no op
        }

        public virtual void OnTriggerExit(Collider other) {
            // no op
        }
    }
}
