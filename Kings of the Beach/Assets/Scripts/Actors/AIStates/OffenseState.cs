using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class OffenseState : AIBaseState
    {
        public OffenseState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ai.TargetPos = ai.OffensePos;
        }

        public override void OnTriggerEnter(Collider other) {
        }

        private void OnTargetSet() {
            ai.StateMachine.ChangeState(ai.DigReadyState);
        }
    }
}
