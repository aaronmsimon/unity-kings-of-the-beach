using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseState : AIBaseState
    {
        public DefenseState(AI ai) : base(ai) { }
        
        private Vector3 targetPos;
        private float estimateRange;
        float halfCourtSide;

        private float blockPos = 1;
        private float defensePos = 6;

        public override void Enter() {
            targetPos = ai.transform.position;

            estimateRange = ai.BallInfo.BallRadius * 2;
            halfCourtSide = ai.CourtSideLength / 2;
            
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ai.TargetPos = targetPos;
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.Ball != null) {
                ai.BlockAttempt();
            }
        }

        private bool MyBall() {
            Athlete teammate = ai.MatchInfo.GetTeammate(ai);
            if (teammate != null) {
                float myDistToBall = (ai.BallInfo.TargetPos - ai.transform.position).sqrMagnitude;
                float teammateDistToBall = (ai.BallInfo.TargetPos - teammate.transform.position).sqrMagnitude;
                return (myDistToBall < teammateDistToBall) || (myDistToBall == teammateDistToBall && ai.Skills.PlayerPosition == PositionType.Defender);
            } else {
                return true;
            }
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
                // probably will need to make different states for receiving a serve and defense during a point, but until then:
                if (Mathf.Abs(ai.transform.position.x) < 2.5f) {
                    ai.PerformJump();
                    return;
                }

                if (MyBall() && JudgeInBounds(ai.Skills.InBoundsJudgement)) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    ai.StateMachine.ChangeState(ai.OffenseState);
                }
            } else {
                // Blocker
                if (ai.Skills.PlayerPosition == PositionType.Blocker) {
                    targetPos = new Vector3(blockPos * ai.CourtSide, ai.transform.position.y, ai.BallInfo.TargetPos.z);
                }
                // Defender
                else {
                    float zBallTargetPos = ai.BallInfo.TargetPos.z;
                    float openSideLength = zBallTargetPos - (-Mathf.Sign(zBallTargetPos) * halfCourtSide);
                    targetPos = new Vector3(defensePos * ai.CourtSide, ai.transform.position.y, zBallTargetPos - openSideLength / 2);
                    Debug.Log($"ball target(z): {zBallTargetPos}, open len: {openSideLength}, target pos: {targetPos}");
                }
            }
        }
    }
}
