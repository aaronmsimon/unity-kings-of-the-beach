using UnityEngine;
using KotB.Items;

public class Bump : MonoBehaviour
{
    public Ball ball;
    
    public void PerformBump(Vector3 targetPos, float height, float duration) {
        ball.BallInfo.SetPassTarget(targetPos, height, duration, null);
        ball.StateMachine.ChangeState(ball.InFlightState);
    }
}
