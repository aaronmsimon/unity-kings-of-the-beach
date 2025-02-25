using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseDefenderState : AIBaseState
    {
        public DefenseDefenderState(AI ai) : base(ai) { }
        
        private Vector3 targetPos;
        private float halfCourtSide;

        private float targetProximity = 0.1f;
        private float defensePos = 6;

        public override void Enter() {
            targetPos = new Vector3(defensePos * ai.CourtSide, ai.transform.position.y, ai.transform.position.z);
            halfCourtSide = ai.CourtSideLength / 2;
            
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ai.TargetPos = targetPos;
            if (Vector3.SqrMagnitude(ai.transform.position - targetPos) < targetProximity * targetProximity) {
                ai.FaceOpponent();
            }
        }

        private void OnTargetSet() {
            // Need to add consideration if a shot is not blockable and Blocker is closer
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                if (ai.JudgeInBounds()) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    targetPos = ai.SetTargetToGiveUp(ai.DistToGiveUp, Random.Range(-0.5f, 0.5f));
                }
            } else {
                float zBallTargetPos = ai.BallInfo.TargetPos.z;
                float openSideLength = zBallTargetPos - (-Mathf.Sign(zBallTargetPos) * halfCourtSide);
                targetPos = new Vector3(defensePos * ai.CourtSide, ai.transform.position.y, zBallTargetPos - openSideLength / 2);
                // Debug.Log($"ball target(z): {zBallTargetPos}, open len: {openSideLength}, target pos: {targetPos}");
            }
        }
    }
}
