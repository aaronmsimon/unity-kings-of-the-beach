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
            if (ai.transform.position != ai.DefensePos) {
                ai.MoveDir = ai.DefensePos - ai.transform.position;
            } else {
                ai.MoveDir = Vector3.zero;
            }
        }

        private bool MyBall() {
            float myDistToBall = (ai.BallInfo.TargetPos - ai.transform.position).sqrMagnitude;
            float teammateDistToBall = (ai.BallInfo.TargetPos - ai.Teammate.transform.position).sqrMagnitude;

            return (myDistToBall < teammateDistToBall) || (myDistToBall == teammateDistToBall && ai.Skills.PlayerPosition == PositionType.Defender);
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
            ai.MoveDir = Vector3.zero;
        }

        private void OnTargetSet() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                if (MyBall()) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    ai.StateMachine.ChangeState(ai.OffenseState);
                }
            }
        }
    }
}
