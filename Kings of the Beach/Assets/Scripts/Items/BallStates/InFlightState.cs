using UnityEngine;
using KotB.Items;

namespace KotB.StatePattern.BallStates
{
    public class InFlightState : BallBaseState
    {
        public InFlightState(Ball ball) : base(ball) { }

        private bool ballIntoNet;
        private float ballSpeedToGround = 5;

        public override void Enter() {
            ball.BallInfo.TimeSinceLastHit = 0f;
            ballIntoNet = false;
        }

        public override void Update() {
            Vector3 targetPos = ball.BallInfo.TargetPos;
            if (ball.transform.position != targetPos && ball.transform.position.y > 0) {
                if (!ballIntoNet) {
                    // Time calculations
                    ball.BallInfo.TimeSinceLastHit += Time.deltaTime;
                    float t = ball.BallInfo.TimeSinceLastHit / ball.BallInfo.Duration;
                    if (t > 1f) t = 1f;

                    // Calculate ball path (spike vs pass)
                    if (ball.BallInfo.Height >= 0) {
                        ball.transform.position = CalculateInFlightPosition(t, ball.BallInfo.StartPos, targetPos, ball.BallInfo.Height);
                    } else {
                        ball.transform.position = CalculateInFlightPosition(t, ball.BallInfo.StartPos, targetPos);
                    }

                    // Check if over the net
                    if (Mathf.Abs(ball.transform.position.x) < 0.13f) {
                        Vector3 moveDir = targetPos - ball.transform.position;
                        CheckIfValid(moveDir);
                        CheckNetCollision(moveDir);
                    }
                } else {
                    ball.transform.position += Vector3.down * ballSpeedToGround * Time.deltaTime;
                }
            } else {
                ball.BallHitGround.Raise();
                ball.StateMachine.ChangeState(ball.GroundState);
                ball.DestroyBallTarget();
            }
        }

        private void CheckIfValid(Vector3 moveDir) {
            if (Physics.Raycast(ball.transform.position, moveDir, out RaycastHit hit, 0.5f, ball.InvalidAimLayer)) {
                ball.BallHitGround.Raise();
                Debug.Log("Ball didn't make it over the net");
            }
        }

        private void CheckNetCollision(Vector3 moveDir) {
            ballIntoNet = Physics.Raycast(ball.transform.position, moveDir, out RaycastHit hit, 0.5f, ball.ObstaclesLayer);
        }

        private Vector3 CalculateInFlightPosition(float t, Vector3 start, Vector3 end, float maxHeightPoint)
        {
            // Linear interpolation for horizontal position (XZ plane)
            Vector3 horizontalPosition = Vector3.Lerp(start, end, t);

            // Parabolic interpolation for vertical position (Y-axis)
                // Using quadratic Bezier curve formula
                float oneMinusT = 1f - t;
                
                // Calculate control point for quadratic curve to pass through maxHeightPoint at t=0.5
                float controlPoint = (maxHeightPoint - (0.25f * start.y) - (0.25f * end.y)) / 0.5f;
                
                // Quadratic interpolation
                float verticalPosition = (oneMinusT * oneMinusT * start.y) + 
                    (2f * oneMinusT * t * controlPoint) + 
                    (t * t * end.y);

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
