using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class OffenseState : AIBaseState
    {
        public OffenseState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.BallInfo.TargetSet += OnTargetSet;
            ai.BallHitGround += OnBallHitGround;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
            ai.BallHitGround -= OnBallHitGround;
        }

        public override void Update() {
            if (ai.transform.position != ai.OffensePos) {
                ai.MoveDir = ai.OffensePos - ai.transform.position;
            } else {
                ai.MoveDir = Vector3.zero;
            }
        }

        public override void OnTriggerEnter(Collider other) {
        }

        private void OnTargetSet() {
            ai.StateMachine.ChangeState(ai.DigReadyState);
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
            ai.MoveDir = Vector3.zero;
        }
    }
}
