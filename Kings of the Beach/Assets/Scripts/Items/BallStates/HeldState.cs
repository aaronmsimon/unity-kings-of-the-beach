using UnityEngine;

namespace KotB.StatePattern.BallStates
{
    public class HeldState : BallBaseState
    {
        public HeldState(Ball ball) : base(ball) { }

        private Vector3 ballHeldPos;
        
        public override void Enter() {
            ball.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ball.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ball.transform.position = ball.BallInfo.ballHeldBy.LeftHandEnd.position;
        }

        private void OnTargetSet() {
            ball.StateMachine.ChangeState(ball.InFlightState);
        }
    }
}
