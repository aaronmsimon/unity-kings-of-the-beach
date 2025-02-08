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
                        ball.transform.position = ball.BallInfo.CalculateInFlightPosition(t, ball.BallInfo.StartPos, targetPos, ball.BallInfo.Height);
                    } else {
                        ball.transform.position = ball.BallInfo.CalculateInFlightPosition(t, ball.BallInfo.StartPos, targetPos);
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
            }
        }

        private void CheckNetCollision(Vector3 moveDir) {
            ballIntoNet = Physics.Raycast(ball.transform.position, moveDir, out RaycastHit hit, 0.5f, ball.ObstaclesLayer);
        }
    }
}
