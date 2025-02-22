using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Actors
{
    [CreateAssetMenu(fileName = "Skill Values", menuName = "Game/Skill Values")]
    public class SkillValues : ScriptableObject
    {
        [Header("Serving")]
        // [SerializeField] private int serving;
        [SerializeField] private MinMax servePower;
        [SerializeField] private MinMax serveAccuracy;

        [Header("Passing")]
        [SerializeField] private MinMax passAccuracy;

        [Header("Spiking")]
        [SerializeField] private MinMax spikePower;
        [SerializeField] private MinMax spikeTimingWindow;

        private MinMax skillRange = new MinMax(1, 10);

        public Vector3 AdjustedServeDirection(Vector3 originalPos, float serving) {
            Vector2 adjAimPos = AdjustVectorAccuracy(new Vector2(originalPos.z, originalPos.y), serving / 10, serveAccuracy, Mathf.Sign(originalPos.x));
            return new Vector3(0, adjAimPos.y, adjAimPos.x);
        }

        public Vector2 AdjustedPassLocation(Vector2 originalLoc, float accuracy, float courtside) {
            return AdjustVectorAccuracy(originalLoc, accuracy, passAccuracy, courtside);
        }

        public float SkillToValue(float skill, MinMax valueRange) {
            return (skill - skillRange.min) * (valueRange.max - valueRange.min) / (skillRange.max - skillRange.min) + valueRange.min;
        }

        private static Vector2 AdjustVectorAccuracy(Vector2 vector, float accuracy, MinMax skillRange, float courtside)
        {
            // Settings
            float netThreshold = 0.51f;
            float ignoreBiasThreshold = 0.2f;

            // Clamp accuracy to the range of 0 to 1 to avoid unexpected results
            accuracy = Mathf.Clamp01(accuracy);
            float deviation = 1f - accuracy;
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(0, (skillRange.max - skillRange.min) * deviation) + skillRange.min;
            
            // If target is close to net and passes a skill check, aim away from net
            if (Mathf.Abs(vector.x) <= netThreshold && Random.value > ignoreBiasThreshold * (1 - accuracy))
            {
                // Calculate angle range that would bias away from net
                Vector3 backCourtDir = Vector3.up * courtside;
                float baseAngle = Mathf.Atan2(backCourtDir.x, backCourtDir.z) * Mathf.Rad2Deg;
                
                // Higher skill = narrower angle range
                float angleRange = Mathf.Lerp(180f, 90f, accuracy);
                float minAngle = baseAngle - (angleRange / 2f);
                float maxAngle = baseAngle + (angleRange / 2f);
                
                // Biased angle away from net
                angle = Random.Range(minAngle, maxAngle);

                Debug.Log($"Angle Adjustment\nAngle: {angle}, Dist: {distance}\nOriginal Target: {vector}, Adjusted Target: {vector + new Vector2((Quaternion.Euler(0, angle, 0) * Vector3.forward * distance).x, (Quaternion.Euler(0, angle, 0) * Vector3.forward * distance).z)}");
            }

            Vector3 adjustment = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
            return vector + new Vector2(adjustment.x, adjustment.z);
        }

        public MinMax ServePower { get { return servePower; } }
        public MinMax ServeAccuracy { get { return serveAccuracy; } }
        public MinMax SpikePower { get { return spikePower; } }
        public MinMax SpikeTimingWindow { get { return spikeTimingWindow; } }
    }
}
