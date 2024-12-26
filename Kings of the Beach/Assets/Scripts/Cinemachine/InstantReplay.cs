using UnityEngine;
using KotB.Items;

namespace KotB.Cinemachine
{
    public class InstantReplay : MonoBehaviour
    {
        private Ball ball;

        private void Awake() {
            ball = GetComponent<Ball>();
        }

        public void Play() {
            Debug.Log($"instant replay to run from {ball.BallInfo.StartPos} to {ball.BallInfo.TargetPos}");
            ball.transform.position = ball.BallInfo.StartPos;
            ball.StateMachine.ChangeState(ball.InFlightState);
        }
    }
}
