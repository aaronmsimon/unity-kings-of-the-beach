using Unity.VisualScripting;
using UnityEngine;

namespace KotB.Actors
{
    public class Teammate : Athlete
    {
        private enum AIState {
            ReceiveServe,
            Offense,
            NetDefense,
            Defend
        }

        [Header("Behavior")]
        [SerializeField][Range(0,1)] private float mySide = 0.5f;
        [SerializeField] private bool showMySide;
        [SerializeField] private Color mySideColor;

        [Header("Teammate")]
        [SerializeField] private SkillsSO teammateSO;

        private int teamSide = -1;
        private float squareLength = 8;
        private float passRangeMin = 0.8f;
        private float passRangeMax = 2.5f;
        private AIState state;

        private void Start() {
            state = AIState.ReceiveServe;
        }

        protected override void Update() {
            if (ballSO == null) return;

            Vector2 ballTarget = new Vector2(ballSO.Target.x, ballSO.Target.z);
            moveDir = Vector3.zero;

            switch (state) {
                case AIState.ReceiveServe:
                    if (ballSO.ballState == BallState.Bump && IsPointWithinMySide(ballTarget)) {
                        moveDir = (ballSO.Target - transform.position).normalized;
                    }
                    break;
                case AIState.Offense:
                    if (ballSO.lastPlayerToHit != this && ballSO.lastPlayerToHit != null) {
                        moveDir = (ballSO.Target - transform.position).normalized;
                    }
                    break;
                case AIState.NetDefense:
                    break;
                case AIState.Defend:
                    break;
                default:
                    Debug.LogError("AI State not handled.");
                    break;
            }

            base.Update(); 
        }

        protected override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
            bumpTimer = 5; // any positive value to trigger the bump
            if (ballSO.HitsForTeam < 2) {
                // pass
                Vector2 teammatePos = new Vector2(teammateSO.Position.x, teammateSO.Position.z);
                Vector2 aimLocation = AdjustVectorAccuracy(teammatePos, skills.PassAccuracy);
                bumpTarget = new Vector3(aimLocation.x, 0f, aimLocation.y);
            } else {
                // shot
                bumpTarget = new Vector3(Random.Range(0, 8), 0f, Random.Range(-4, 4));
            }
        }

        private bool IsPointWithinMySide(Vector2 point)
        {
            Vector2 min = new Vector2(-8, squareLength / 2 - squareLength * mySide);
            Vector2 max = new Vector2(0, 4);

            // Check if the point's x is between minX and maxX
            bool withinX = point.x >= min.x && point.x <= max.x;

            // Check if the point's y is between minY and maxY
            bool withinY = point.y >= min.y && point.y <= max.y;

            // Return true if both x and y are within bounds
            return withinX && withinY;
        }

        private bool IsPointOnTeamSide(Vector2 point) {
            return (point.x < 0 && teamSide < 0) || (point.x > 0 && teamSide > 0);
        }

        private Vector2 AdjustVectorAccuracy(Vector2 vector, float accuracy)
        {
            // Clamp accuracy to the range of 0 to 1 to avoid unexpected results
            accuracy = Mathf.Clamp01(accuracy);

            // Calculate the maximum deviation based on the accuracy
            float deviation = 1 - accuracy;

            // Generate a random unit vector (random direction)
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Generate a random magnitude based on the deviation
            // float randomMagnitude = Random.Range(0f, deviation);
            float randomMagnitude = Random.Range(0, (passRangeMax - passRangeMin) * deviation) + passRangeMin;
            Debug.Log(randomMagnitude);

            // Calculate the random offset
            Vector2 randomOffset = randomDirection * randomMagnitude;

            // Add the random offset to the original vector
            return vector + randomOffset;
        }

        private void OnDrawGizmos() {
            if (showMySide) {
                Vector2 mySideArea = new Vector2(squareLength, squareLength * mySide);
                Vector3 areaCenter = new Vector3(-mySideArea.x / 2, 0, squareLength / 2 - mySideArea.y / 2);
                Vector3 areaSize = new Vector3(mySideArea.x, 0.1f, mySideArea.y);

                Gizmos.color = mySideColor;
                Gizmos.DrawCube(areaCenter, areaSize);
            }
        }
    }
}
