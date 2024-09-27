using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseState : AIBaseState
    {
        public DefenseState(AI ai) : base(ai) { }

        public override void Enter() {
            ai.BallHitGround += OnBallHitGround;
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallHitGround -= OnBallHitGround;
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            Vector3 receivingPos = new Vector3(ai.Skills.ReceivingPos.x * ai.CourtSide, ai.Skills.ReceivingPos.y, ai.Skills.ReceivingPos.z);
            if (ai.transform.position != receivingPos) {
                ai.MoveDir = receivingPos - ai.transform.position;
            } else {
                ai.MoveDir = Vector3.zero;
            }
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
        }

        private void OnTargetSet() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                ai.StateMachine.ChangeState(ai.DigReadyState);
            }
        }
    }
}
