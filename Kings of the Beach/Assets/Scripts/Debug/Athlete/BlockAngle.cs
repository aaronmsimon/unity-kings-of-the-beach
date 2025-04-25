using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using KotB.Items;
using KotB.Stats;

namespace KotB.Testing
{
    public class BlockAngle : MonoBehaviour
    {
        [SerializeField] private FloatVariable courtSide;
        [SerializeField] private SkillsSO skills;
        [SerializeField] private BallSO ballInfo;
        
        private Vector3 colliderCenter;
        private CollisionTriggerReporter collisionTrigger;
        private bool blockAttempted;

        private void Awake() {
            colliderCenter = transform.position;
            collisionTrigger = GetComponent<CollisionTriggerReporter>();
        }

        private void OnEnable() {
            collisionTrigger.Triggered += OnBlock;
            collisionTrigger.Active = true;
        }

        private void OnDisable() {
            collisionTrigger.Triggered -= OnBlock;
        }

        public void OnReset() {
            blockAttempted = false;
        }

        private void OnBlock(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                if (!blockAttempted) {
                    Vector3 contactPoint = collisionTrigger.TriggerCollider.ClosestPoint(ball.transform.position);
                    BlockInitiated(contactPoint);
                    blockAttempted = true;
                }
            }
        }

        private void BlockInitiated(Vector3 contactPoint) {
            // Use the stored contact point for more accurate quality calculation
            Vector3 contactDirection = contactPoint - (transform.position + colliderCenter);
            float contactQuality = Vector3.Dot(contactDirection.normalized, Vector3.right * -courtSide.Value);
            contactQuality = Mathf.Clamp01(contactQuality);

            Vector3 spikeDir = contactPoint - ballInfo.StartPos;
            Vector3 spikeDirXZ = new Vector3(spikeDir.x, 0, spikeDir.z).normalized;
            Vector3 blockNormal = Vector3.right * -courtSide.Value;
            Vector3 reflectDir = Vector3.Reflect(spikeDirXZ, blockNormal);
            Vector3 bounceDir = new Vector3(reflectDir.x, 0, reflectDir.z).normalized;
            float contactAngle = Vector3.Angle(spikeDirXZ, -blockNormal);
            Debug.DrawLine(new Vector3(ballInfo.StartPos.x, contactPoint.y, ballInfo.StartPos.z), contactPoint, Color.yellow, 10f, false);
            Debug.DrawLine(contactPoint, contactPoint + bounceDir * 3, Color.green, 10f, false);
            Debug.DrawLine(contactPoint, contactPoint + blockNormal  * 3, Color.red, 10f, false);
            
            float incomingAngle = Vector3.Angle(blockNormal, spikeDirXZ);
            float outgoingAngle = Vector3.Angle(blockNormal, bounceDir);
            Debug.Log($"incoming angle: {incomingAngle}, outgoing angle: {outgoingAngle}");

            // Determine if it's a strong block (spike) or a soft block (pass)
            bool strongBlock = contactAngle <= 45;
            Debug.Log($"{contactAngle} <= 45? -> {(strongBlock ? "Strong" : "Weak")} Block at y = {contactPoint.y}");
            float powerReduction = 0.5f;
            float maxBlockHeight = 5;

            float targetDistance = Mathf.Lerp(2f, 4f, contactQuality) * (strongBlock ? 1 : (1 - powerReduction));
            // TargetPos is more consistent with Kings of the Beach (Athlete's z-pos), but might want to actually use angles if wanting to add more realism
            Vector3 targetPos = new Vector3(targetDistance * -courtSide.Value, 0.01f, transform.position.z);
            Debug.Log($"Target Distance: Lerp(2,4,{contactQuality})={targetDistance}");
            // Debug.DrawLine(contactPoint, targetPos, Color.red, 10f, false);
            float blockDuration;
            
            if (strongBlock) {
                // Strong blocks are like spikes - faster and more direct
                blockDuration = Mathf.Lerp(ballInfo.SkillValues.BlockPower.min, ballInfo.SkillValues.BlockPower.max, skills.BlockPower);
                ballInfo.SetSpikeTarget(targetPos, blockDuration, null, StatTypes.Block);
            } else {
                // Weak blocks are like passes - slower and higher
                float blockHeight = Mathf.Lerp(ballInfo.Position.y, maxBlockHeight, contactQuality);
                blockDuration = Mathf.Lerp(1.5f, 2.5f, contactQuality);
                ballInfo.SetSpikeTarget(targetPos + Vector3.right * powerReduction * -courtSide.Value, blockDuration, null, StatTypes.Block, blockHeight);
            }
            
            // Reset hits for this team
/* disbling for testing
            ballInfo.HitsForTeam = 0;
            ballInfo.StatUpdate.Raise(this, StatTypes.Block);
*/
            
            // Log for debugging
            // Debug.Log($"Block by {skills.AthleteName}: Contact Point={lastBlockContactPoint}, AthletePos={transform.position}, ColliderPos={spikeBlockCollider.center}, " +
            //         $"Direction={contactDirection} Quality={contactQuality}, Angle={contactAngle} ({(strongBlock ? "Strong" : "Weak")}) " +
            //         $"Target={targetPos} (Distance={targetDistance}, Duration={blockDuration} " +
            //         $"{(!strongBlock ? "Height=" + Mathf.Lerp(ball.transform.position.y, maxBlockHeight, contactQuality) : "")})"
            // );
        }
    }
}
