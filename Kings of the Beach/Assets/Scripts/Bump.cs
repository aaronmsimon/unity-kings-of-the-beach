using UnityEngine;

public class Bump : MonoBehaviour
{
    public Ball ball;
    
    public void PerformBump(Vector3 targetPos, float height, float duration) {
        // ball.BallInfo.SetPassTarget(targetPos, height, duration);
        ball.StateMachine.ChangeState(ball.InFlightState);
    }
}
