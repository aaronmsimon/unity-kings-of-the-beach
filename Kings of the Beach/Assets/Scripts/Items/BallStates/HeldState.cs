using UnityEngine;

namespace KotB.StatePattern.BallStates
{
    public class HeldState : BallBaseState
    {
        public HeldState(Ball ball) : base(ball) { }

        private float athleteColliderWidth = .35f;
        private float ballHeldHeight = 1.09f;
        
        public override void Enter() {
            ball.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ball.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ball.transform.position = ball.BallInfo.ballHeldBy.transform.position + new Vector3(ball.BallInfo.ballHeldBy.CourtSide * -athleteColliderWidth, ballHeldHeight, 0);
        }

        private void OnTargetSet() {
            ball.StateMachine.ChangeState(ball.InFlightState);
        }
    }
}
