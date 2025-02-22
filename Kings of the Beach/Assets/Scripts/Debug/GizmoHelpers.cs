using UnityEngine;

namespace Cackenballz.Helpers
{
    public static class GizmoHelpers
    {
        public static void DrawGizmoCircle(Vector3 center, float radius, Color color, int segments)
        {
            Gizmos.color = color;
            const float two_pi = Mathf.PI * 2;
            float step = two_pi / (float)segments;
            float theta = 0;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos = center + new Vector3(x, 0, y);
            Vector3 newPos;
            Vector3 lastPos = pos;
            for (theta = step; theta < two_pi; theta += step)
            {
                x = radius * Mathf.Cos(theta);
                y = radius * Mathf.Sin(theta);
                newPos = center + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);
        }

        public static void DrawGizmoArc(Vector3 center, float radius, float startAngleDegrees, float endAngleDegrees, Color color, int segments = 20)
        {
            Gizmos.color = color;
            
            // Convert angles to radians and normalize them
            float startAngle = startAngleDegrees * Mathf.Deg2Rad;
            float endAngle = endAngleDegrees * Mathf.Deg2Rad;
            
            // Ensure the end angle is greater than the start angle
            if (endAngle < startAngle)
            {
                endAngle += 2 * Mathf.PI;
            }
            
            // Calculate how many segments we need based on the angle difference
            float angleDiff = endAngle - startAngle;
            int segmentsToUse = Mathf.Max(1, Mathf.RoundToInt((angleDiff / (2 * Mathf.PI)) * segments));
            float step = angleDiff / segmentsToUse;
            
            // Draw the arc segments
            Vector3 lastPoint = center + new Vector3(
                radius * Mathf.Cos(startAngle),
                0,
                radius * Mathf.Sin(startAngle)
            );
            
            // Draw first line from center to arc start
            Gizmos.DrawLine(center, lastPoint);
            
            // Draw the arc
            for (float theta = startAngle + step; theta <= endAngle + 0.0001f; theta += step)
            {
                Vector3 nextPoint = center + new Vector3(
                    radius * Mathf.Cos(theta),
                    0,
                    radius * Mathf.Sin(theta)
                );
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
            
            // Draw last line back to center
            Gizmos.DrawLine(lastPoint, center);
        }
    }
}
