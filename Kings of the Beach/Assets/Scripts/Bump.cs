using UnityEngine;

public class Bump : MonoBehaviour
{
    [SerializeField] private Ball ball;
    
    public void PerformBump(Vector3 targetPos, float height, float duration) {
        ball.Bump(targetPos, height, duration);
    }
}
