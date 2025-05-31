using KotB.Items;

namespace KotB.StatePattern.BallStates
{
    public class GroundState : BallBaseState
    {
        public GroundState(Ball ball) : base(ball) { }

        public override void Enter() {
            ball.BallInfo.InPlay = false;
        }
    }
}
