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
    }
}
