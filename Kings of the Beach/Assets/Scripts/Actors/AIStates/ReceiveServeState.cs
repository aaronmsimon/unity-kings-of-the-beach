using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class ReceiveServeState : AIBaseState
    {
        public ReceiveServeState(AI ai) : base(ai) { }
        
        public override void Enter() {
            ai.transform.position = ai.ServeDefPos;
            ai.FaceOpponent();
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
            ai.TargetPos = ai.SetTargetToGiveUp(ai.DistToGiveUp, Random.Range(-0.5f, 0.5f));
        }
    }
}
