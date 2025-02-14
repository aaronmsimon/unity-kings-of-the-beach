using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseDefenderState : AIBaseState
    {
        public DefenseDefenderState(AI ai) : base(ai) { }
        
        private Vector3 targetPos;
        float halfCourtSide;

        private float defensePos = 6;
        private float distToGiveUp = 1;

        public override void Enter() {
            halfCourtSide = ai.CourtSideLength / 2;
            
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ai.TargetPos = targetPos;
        }

        private void OnTargetSet() {
            // Need to add consideration if a shot is not blockable and Blocker is closer
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                if (ai.JudgeInBounds()) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    targetPos = Vector3.Lerp(ai.transform.position, ai.BallInfo.TargetPos, 1 - (distToGiveUp / Vector3.Distance(ai.transform.position, ai.BallInfo.TargetPos)));
                }
            } else {
                float zBallTargetPos = ai.BallInfo.TargetPos.z;
                float openSideLength = zBallTargetPos - (-Mathf.Sign(zBallTargetPos) * halfCourtSide);
                targetPos = new Vector3(defensePos * ai.CourtSide, ai.transform.position.y, zBallTargetPos - openSideLength / 2);
                Debug.Log($"ball target(z): {zBallTargetPos}, open len: {openSideLength}, target pos: {targetPos}");
            }
        }
    }
}
