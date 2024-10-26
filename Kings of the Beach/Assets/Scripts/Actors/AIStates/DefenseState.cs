using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseState : AIBaseState
    {
        public DefenseState(AI ai) : base(ai) { }
        
        private float estimateRange;

        public override void Enter() {
            estimateRange = ai.BallInfo.BallRadius * 2;
            
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
                ai.transform.forward = Vector3.right * -ai.CourtSide;
            }
        }

        private bool MyBall() {
            float myDistToBall = (ai.BallInfo.TargetPos - ai.transform.position).sqrMagnitude;
            float teammateDistToBall = (ai.BallInfo.TargetPos - ai.Teammate.transform.position).sqrMagnitude;

            return (myDistToBall < teammateDistToBall) || (myDistToBall == teammateDistToBall && ai.Skills.PlayerPosition == PositionType.Defender);
        }

        private bool JudgeInBounds(float skillLevel) {
            // get a random value on the skill level scale
            float randValue = Random.value * ai.SkillLevelMax;
            // skill check
            bool accurateEstimate = randValue <= skillLevel;
            // get actual in bounds value
            bool actualInBounds = ai.BallInfo.IsInBounds(ai.BallInfo.TargetPos);
            // check if it's close
            bool closeInBounds =
                (Mathf.Abs(ai.BallInfo.TargetPos.x) >= ai.CourtSideLength - estimateRange && Mathf.Abs(ai.BallInfo.TargetPos.x) <= ai.CourtSideLength + estimateRange) ||
                (Mathf.Abs(ai.BallInfo.TargetPos.z) >= ai.CourtSideLength / 2 - estimateRange && Mathf.Abs(ai.BallInfo.TargetPos.z) <= ai.CourtSideLength / 2 + estimateRange);
            //                                                                             ↓ not close - anyone knows the right result
            return closeInBounds ? (accurateEstimate ? actualInBounds : !actualInBounds) : actualInBounds;
            //     ↑ if it's close  ↑ if passed skill chk,  correct ↑    ↑ otherwise, wrong
            
        }

        private void OnBallHitGround() {
            ai.StateMachine.ChangeState(ai.PostPointState);
            ai.MoveDir = Vector3.zero;
        }

        private void OnTargetSet() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                if (MyBall() && JudgeInBounds(ai.Skills.InBoundsJudgement)) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    ai.StateMachine.ChangeState(ai.OffenseState);
                }
            }
        }
    }
}
