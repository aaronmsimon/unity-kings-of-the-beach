using KotB.Items;

namespace KotB.StatePattern.BallStates
{
    public class HeldState : BallBaseState
    {
        public HeldState(Ball ball) : base(ball) { }

        public override void Update() {
            ball.transform.position = ball.BallInfo.BallHeldBy.LeftHandEnd.position;
        }
    }
}
