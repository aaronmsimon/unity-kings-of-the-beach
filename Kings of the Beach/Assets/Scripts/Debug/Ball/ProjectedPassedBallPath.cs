using KotB.Items;
using UnityEngine;

namespace KotB.Testing
{
    public class ProjectedPassedBallPath : MonoBehaviour
    {
        [SerializeField] private Transform spikePoint;
        [SerializeField] private Transform target;
        [SerializeField] private float maxHeight;
        [SerializeField] private float gizmoCount;
        [SerializeField] private float gizmoRadius = 0.25f;
        [SerializeField] private Color gizmoColor = Color.red;
        [SerializeField][Range(1,10)] private float spikeSkill;
        [SerializeField] private BallSO ballInfo;

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

        private void OnDrawGizmos() {
            if (spikePoint != null && target != null && ballInfo != null) {
                Gizmos.color = gizmoColor;
                for (int i = 0; i < gizmoCount; i++) {
                    Gizmos.DrawSphere(CalculateInFlightPosition(i / gizmoCount, spikePoint.position, target.position, maxHeight), gizmoRadius);
                }
            }
        }
    }
}
