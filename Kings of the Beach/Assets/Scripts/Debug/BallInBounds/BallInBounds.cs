using UnityEngine;
using KotB.Items;

namespace KotB.Testing
{
    public class BallInBounds : MonoBehaviour
    {
        private Ball ball;

        private void Awake() {
            ball = GetComponent<Ball>();
        }

        public void CheckInBounds() {
            Debug.Log($"Ball at position {ball.BallInfo.Position} is {(ball.BallInfo.IsInBounds(ball.BallInfo.Position) ? "in" : "out of")} bounds.");
        }
    }
}
