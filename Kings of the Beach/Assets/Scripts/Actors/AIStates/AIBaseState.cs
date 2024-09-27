using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public abstract class AIBaseState : IState
    {
        protected readonly AI ai;

        protected AIBaseState(AI context) {
            ai = context;
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
