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
            
            ai.BallInfo.TargetSet += OnTargetSet;
            ai.ReachedTargetPos += OnReachedTargetPos;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
            ai.ReachedTargetPos -= OnReachedTargetPos;
        }

        public override void Update() {
            ai.TargetPos = ai.DefensePos;
        }

        public override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
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

        private void OnTargetSet() {
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                if (MyBall() && JudgeInBounds(ai.Skills.InBoundsJudgement)) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    ai.StateMachine.ChangeState(ai.OffenseState);
                }
            }
        }

        private void OnReachedTargetPos() {
            ai.transform.forward = Vector3.right * -ai.CourtSide;
        }
    }
}
