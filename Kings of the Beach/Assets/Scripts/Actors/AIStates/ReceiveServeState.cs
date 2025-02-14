using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ReceiveServeState : AIBaseState
    {
        public ReceiveServeState(AI ai) : base(ai) { }
        
        private Vector3 targetPos;

        public override void Enter() {
            ai.ReachedTargetPos += OnReachedTargetPos;
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.ReachedTargetPos -= OnReachedTargetPos;
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        private void OnReachedTargetPos() {
            ai.transform.rotation = Quaternion.LookRotation(Vector3.right * -ai.CourtSide);
        }

        private void OnTargetSet() {
            if (ai.MyBall()) {
                if (ai.JudgeInBounds()) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    ai.SetTargetToGiveUp();
                }
            } else {
                ai.StateMachine.ChangeState(ai.OffenseState);
            }
        }
    }
}
