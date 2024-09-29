using UnityEngine;

namespace KotB.StatePattern.BallStates
{
    public class InFlightState : BallBaseState
    {
        public InFlightState(Ball ball) : base(ball) { }

        public override void Enter() {
            ball.BallInfo.TimeSinceLastHit = 0f;
        }

        public override void Update() {
            Vector3 targetPos = ball.BallInfo.TargetPos;
            if (ball.transform.position != targetPos) {
                ball.BallInfo.TimeSinceLastHit += Time.deltaTime;
                float t = ball.BallInfo.TimeSinceLastHit / ball.BallInfo.Duration;
                if (t > 1f) t = 1f;
                if (t >= 0.5f && !ball.BallInfo.ApexReachedFlag) {
                    ball.BallInfo.ApexReachedEvent();
                    ball.BallInfo.ApexReachedFlag = true;
                }

                if (ball.BallInfo.Height >= 0) {
                    ball.transform.position = CalculateInFlightPosition(t, ball.BallInfo.StartPos, targetPos, ball.BallInfo.Height);
                } else {
                    ball.transform.position = CalculateInFlightPosition(t, ball.BallInfo.StartPos, targetPos);
                }
            } else {
                ball.BallHitGround.Raise();
                ball.StateMachine.ChangeState(ball.GroundState);
                ball.DestroyBallTarget();
            }
        }

        private Vector3 CalculateInFlightPosition(float t, Vector3 start, Vector3 end, float height)
        {
            // Linear interpolation for horizontal position (XZ plane)
            Vector3 horizontalPosition = Vector3.Lerp(start, end, t);

            // Parabolic interpolation for vertical position (Y-axis)
            float verticalPosition = 4 * height * t * (1 - t) + Mathf.Lerp(start.y, end.y, t);

            // Combine horizontal and vertical movement
            horizontalPosition.y = verticalPosition;

            return horizontalPosition;
        }

        private Vector3 CalculateInFlightPosition(float t, Vector3 start, Vector3 end) {
            // Linear interpolation for horizontal position (XZ plane)
            return Vector3.Lerp(start, end, t);
        }
    }
}
