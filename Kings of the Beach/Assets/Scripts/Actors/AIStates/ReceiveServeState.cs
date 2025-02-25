using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ReceiveServeState : AIBaseState
    {
        public ReceiveServeState(AI ai) : base(ai) { }
        
        public override void Enter() {
            ai.TargetPos = ai.transform.position;
            
            ai.ReachedTargetPos += OnReachedTargetPos;
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.ReachedTargetPos -= OnReachedTargetPos;
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        private void OnReachedTargetPos() {
            ai.FaceOpponent();
        }

        private void OnTargetSet() {
            if (ai.MyBall()) {
                if (ai.JudgeInBounds()) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                } else {
                    ai.SetTargetToGiveUp(1, Random.Range(-0.5f, 0.5f));
                }
            } else {
                ai.StateMachine.ChangeState(ai.OffenseState);
            }
        }
    }
}
