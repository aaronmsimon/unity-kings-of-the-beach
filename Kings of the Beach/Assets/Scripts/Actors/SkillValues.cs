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
            return AdjustVectorAccuracy(originalLoc, accuracy, passAccuracy, 4f);
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

        private static Vector2 AdjustVectorAccuracy(Vector2 vector, float accuracy, MinMax skillRange, float netBias)
        {
            accuracy = Mathf.Clamp01(accuracy);
            float deviation = 1 - accuracy;
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            string debugMsg = $"Random Direction: {randomDirection}";

            // Calculate how close to the net we are (0 = at net, 1 = far from net)
            float distanceFromNet = Mathf.Abs(vector.x);
            float netProximity = Mathf.Clamp01(distanceFromNet / 2.5f); // Percentage of court size
            
            // If we're close to the net, adjust only the x component of the random direction
            if (distanceFromNet < 2.5f)
            {
                // Create a bias x-component pointing away from the net
                float biasX = Mathf.Sign(vector.x);
                
                // Lerp between the random x-component and the bias x-component based on net proximity
                float biasStrength = 1f - (netProximity * netBias); // More bias when closer to net
                float newX = Mathf.Lerp(randomDirection.x, biasX, biasStrength);
                
                // Keep the y component unchanged
                randomDirection = new Vector2(newX, randomDirection.y).normalized;
                debugMsg += $" bias update -> netProx: {netProximity}, bias: {biasStrength} => new Random Direction: {randomDirection} (x only={newX})";
            }
            Debug.Log(debugMsg);

            float randomMagnitude = Random.Range(0, (skillRange.max - skillRange.min) * deviation) + skillRange.min;
            Vector2 randomOffset = randomDirection * randomMagnitude;

            return vector + randomOffset;
        }

        public MinMax ServePower { get { return servePower; } }
        public MinMax ServeAccuracy { get { return serveAccuracy; } }
        public MinMax SpikePower { get { return spikePower; } }
        public MinMax SpikeTimingWindow { get { return spikeTimingWindow; } }
    }
}
