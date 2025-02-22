using UnityEngine;
using Cackenballz.Helpers;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Testing
{
    public class VectorAccuracyVisual : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private int segments = 8;

        [Header("Settings")]
        [SerializeField][Range(0,1)] private float accuracySkill;
        [SerializeField] private FloatVariable courtside;
        [SerializeField] private float maxOffset;
        [SerializeField] private float netProximity;
        [SerializeField] private float ignoreBiasThreshold;
        [SerializeField][Range(0,1)] private float randValueBias;

        private void OnDrawGizmos() {
            CalculatePassTarget(transform.position, accuracySkill);
        }

        public void CalculatePassTarget(Vector3 targetPosition, float skillLevel)
        {
            float offset = (1f - skillLevel) * maxOffset; // Base maximum offset scaled by inverse skill
            
            // If target is close to net (within 0.5 units)
            if (Mathf.Abs(targetPosition.x) <= netProximity)
            {
                // Calculate angle range that would bias away from net
                Vector3 backCourtDir = Vector3.up * courtside.Value;
                float baseAngle = Mathf.Atan2(backCourtDir.x, backCourtDir.z) * Mathf.Rad2Deg;
                
                // Higher skill = narrower angle range
                float angleRange = Mathf.Lerp(180f, 90f, skillLevel);
                float minAngle = baseAngle - (angleRange / 2f);
                float maxAngle = baseAngle + (angleRange / 2f);
                
                // Small chance to ignore the bias (creates unpredictability)
                // if (Random.value < ignoreBiasThreshold * skillLevel)
                if (randValueBias < ignoreBiasThreshold * (1 - skillLevel))
                {
                    // Full 360 random offset
                    // float randomAngle = Random.Range(0f, 360f);
                    // float randomDistance = Random.Range(0f, maxOffset);
                    GizmoHelpers.DrawGizmoCircle(targetPosition, offset, Color.red, segments);
                }
                else
                {
                    // Biased angle away from net
                    // float biasedAngle = Random.Range(minAngle, maxAngle);
                    // float randomDistance = Random.Range(0f, maxOffset);
                    GizmoHelpers.DrawGizmoArc(targetPosition, offset, minAngle, maxAngle, Color.black);
                }
            }
            else
            {
                // Normal 360 degree random offset for targets far from net
                // float randomAngle = Random.Range(0f, 360f);
                // float randomDistance = Random.Range(0f, maxOffset);
                GizmoHelpers.DrawGizmoCircle(targetPosition, offset, Color.blue, segments);
            }
        }
    }
}
