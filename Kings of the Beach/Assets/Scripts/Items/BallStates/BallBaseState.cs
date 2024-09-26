using UnityEngine;

namespace KotB.StatePattern.BallStates
{
    public abstract class BallBaseState : IState
    {
        protected readonly Ball ball;

        protected BallBaseState(Ball context) {
            ball = context;
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
