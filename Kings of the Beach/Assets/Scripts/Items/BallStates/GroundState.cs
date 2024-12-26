using UnityEngine;
using KotB.Items;

namespace KotB.StatePattern.BallStates
{
    public class GroundState : BallBaseState
    {
        public GroundState(Ball ball) : base(ball) { }

        public override void Enter() {
            ball.BallInfo.BallGiven += OnBallGiven;
        }

        public override void Exit() {
            ball.BallInfo.BallGiven -= OnBallGiven;
        }

        public void OnBallGiven() {
            ball.StateMachine.ChangeState(ball.HeldState);
        }
    }
}
