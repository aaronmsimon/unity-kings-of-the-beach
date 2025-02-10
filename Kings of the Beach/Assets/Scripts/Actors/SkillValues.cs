using UnityEngine;

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
            Vector2 adjAimPos = AdjustVectorAccuracy(new Vector2(originalPos.z, originalPos.y), serving / 10, serveAccuracy);
            return new Vector3(0, adjAimPos.y, adjAimPos.x);
        }

        public Vector2 AdjustedPassLocation(Vector2 originalLoc, float accuracy) {
            return AdjustVectorAccuracy(originalLoc, accuracy, passAccuracy);
        }

        public float SkillToValue(float skill, MinMax valueRange) {
            return (skill - skillRange.min) * (valueRange.max - valueRange.min) / (skillRange.max - skillRange.min) + valueRange.min;
        }

        private static Vector2 AdjustVectorAccuracy(Vector2 vector, float accuracy, MinMax skillRange)
        {
            // Clamp accuracy to the range of 0 to 1 to avoid unexpected results
            accuracy = Mathf.Clamp01(accuracy);

            // Calculate the maximum deviation based on the accuracy
            float deviation = 1 - accuracy;

            // Generate a random unit vector (random direction)
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Generate a random magnitude based on the deviation
            // float randomMagnitude = Random.Range(0f, deviation);
            float randomMagnitude = Random.Range(0, (skillRange.max - skillRange.min) * deviation) + skillRange.min;

            // Calculate the random offset
            Vector2 randomOffset = randomDirection * randomMagnitude;

            // Add the random offset to the original vector
            return vector + randomOffset;
        }

        public MinMax ServePower { get { return servePower; } }
        public MinMax ServeAccuracy { get { return serveAccuracy; } }
        public MinMax SpikePower { get { return spikePower; } }
        public MinMax SpikeTimingWindow { get { return spikeTimingWindow; } }
    }
}
