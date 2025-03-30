using UnityEngine;
using KotB.Actors;
using KotB.Items;
using KotB.Stats;

namespace KotB.Testing
{
    public class BlockTargetPos : MonoBehaviour
    {
        [SerializeField] private Athlete athlete;
        [SerializeField] private BallSO ballInfo;
        [SerializeField] private Transform lastBlockContactPoint;
        [SerializeField][Range(1,10)] private float skillBlocking;
        
        private float skillLevelMax = 10;

        private void Block() {
            // Use the stored contact point for more accurate quality calculation
            SphereCollider sphereCollider = (SphereCollider)athlete.SpikeTrigger.Collider;
            Vector3 contactDirection = lastBlockContactPoint.position - (transform.position + sphereCollider.center);
            float contactQuality = Vector3.Dot(contactDirection.normalized, transform.right * -athlete.CourtSide);
            contactQuality = Mathf.Clamp01(contactQuality);
            
            // Calculate block effectiveness based on skill and contact quality
            float blockSkill = skillBlocking / skillLevelMax;
            float blockEffectiveness = blockSkill * 0.7f + contactQuality * 0.3f;
            
            // Add some randomness
            float randomFactor = UnityEngine.Random.Range(0.8f, 1.2f);
            blockEffectiveness *= randomFactor;
            
            // Determine if it's a strong block (spike) or a soft block (pass)
            bool strongBlock = blockEffectiveness > 0.65f;
            
            // Calculate target position - further for better blocks
            float targetDistance = Mathf.Lerp(2f, 6f, blockEffectiveness);
            Vector3 targetPos = new Vector3(targetDistance * -athlete.CourtSide, 0.01f, transform.position.z);
            
            if (strongBlock) {
                // Strong blocks are like spikes - faster and more direct
                float blockDuration = Mathf.Lerp(1.5f, 0.8f, blockEffectiveness);
                ballInfo.SetSpikeTarget(targetPos, blockDuration, athlete, StatTypes.Block);
            } else {
                // Weak blocks are like passes - slower and higher
                float blockHeight = Mathf.Lerp(2f, 4f, blockEffectiveness);
                float blockDuration = Mathf.Lerp(2.5f, 1.5f, blockEffectiveness);
                ballInfo.SetPassTarget(targetPos, blockHeight, blockDuration, athlete, StatTypes.Block);
            }
            
            // Log for debugging
            Debug.Log($"Block by {athlete.Skills.AthleteName}: Effectiveness={blockEffectiveness:F2}, " +
                    $"Type={(strongBlock ? "Strong" : "Weak")}, Target={targetPos}");
        }

        private void OnDrawGizmos() {
            SphereCollider sphereCollider = (SphereCollider)athlete.SpikeTrigger.Collider;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(lastBlockContactPoint.position, lastBlockContactPoint.position - (transform.position + sphereCollider.center));

            Gizmos.color = Color.magenta;
            // Gizmos.DrawLine(lastBlockContactPoint.position, lastBlockContactPoint.position - (transform.position + sphereCollider.center));
        }
    }
}
