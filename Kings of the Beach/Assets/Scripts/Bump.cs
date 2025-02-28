using UnityEngine;
using KotB.Items;
using KotB.Stats;

public class Bump : MonoBehaviour
{
    public Ball ball;
    
    public void PerformBump(Vector3 targetPos, float height, float duration) {
        ball.BallInfo.SetPassTarget(targetPos, height, duration, null, StatTypes.None);
        ball.StateMachine.ChangeState(ball.InFlightState);
    }
}
