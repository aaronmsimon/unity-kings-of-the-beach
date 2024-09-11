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
        [SerializeField][Range(0,1)] private float mySidePct = 0.5f;
        [SerializeField] private float myZoneFront = 1.5f;
        [SerializeField] private float myZoneBack = 2f;
        [SerializeField] private bool showMySide;
        [SerializeField] private Color mySideColor;

        [Header("Teammate")]
        [SerializeField] private SkillsSO teammateSO;

        private float courtSideLength = 8;
        private float passRangeMin = 0.8f;
        private float passRangeMax = 2.5f;
        private AIState state;
        private Vector2 myZoneTopLeft;
        private Vector2 myZoneBotRight;

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
                    UpdateMyZone();
                    break;
                case AIState.Offense:
                    if (ballSO.lastPlayerToHit != this && ballSO.lastPlayerToHit != null) {
                        moveDir = (ballSO.Target - transform.position).normalized;
                    }
                    break;
                case AIState.NetDefense:
                    UpdateMyZone();
                    break;
                case AIState.Defend:
                    UpdateMyZone();
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

        private void UpdateMyZone() {
            // If on the correct side
            if (Mathf.Sign(transform.position.x) == courtSide) {
                // Check if at front of court
                if (Mathf.Abs(transform.position.x) <= myZoneFront) {
                    myZoneTopLeft = new Vector2(courtSide * (myZoneFront / 2) + (myZoneFront / -2), courtSideLength / 2);
                    myZoneBotRight = new Vector2(courtSide * (myZoneFront / 2) + (myZoneFront / 2), -courtSideLength / 2);
                }
                // Check if at back of court
                else if (Mathf.Abs(transform.position.x) >= courtSideLength - myZoneBack) {
                    myZoneTopLeft = new Vector2(courtSide * (courtSideLength + myZoneBack / -2) + (myZoneBack / -2), courtSideLength / 2);
                    myZoneBotRight = new Vector2(courtSide * (courtSideLength + myZoneBack / -2) + (myZoneBack / 2), -courtSideLength / 2);
                }
                // If not near front nor back, then take half based on which half they're on
                else {
                        float startYpos = transform.position.z > 0 ? courtSideLength / 2 : 0;
                        float halfCourtSideLength = courtSideLength / 2;
                        myZoneTopLeft = new Vector2(courtSide * halfCourtSideLength - halfCourtSideLength, startYpos);
                        myZoneBotRight = new Vector2(courtSide * halfCourtSideLength + halfCourtSideLength, startYpos - (courtSideLength * mySidePct));
                }
            } else {
                myZoneTopLeft = Vector2.zero;
                myZoneBotRight = Vector2.zero;
            }
        }

        private bool IsPointWithinMySide(Vector2 point)
        {
            // Check if the point's x is between minX and maxX
            bool withinX = point.x >= myZoneTopLeft.x && point.x <= myZoneBotRight.x;

            // Check if the point's y is between minY and maxY
            bool withinY = point.y >= myZoneBotRight.y && point.y <= myZoneTopLeft.y;

            // Return true if both x and y are within bounds
            return withinX && withinY;
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

            // Calculate the random offset
            Vector2 randomOffset = randomDirection * randomMagnitude;

            // Add the random offset to the original vector
            return vector + randomOffset;
        }

        private void OnDrawGizmos() {
            if (showMySide) {
                UpdateMyZone();
                Vector3 areaCenter = new Vector3((myZoneTopLeft.x + myZoneBotRight.x) / 2, 0f, (myZoneTopLeft.y + myZoneBotRight.y) / 2);
                Vector3 areaSize = new Vector3(myZoneBotRight.x - myZoneTopLeft.x, 0.1f, myZoneTopLeft.y - myZoneBotRight.y);

                Gizmos.color = mySideColor;
                Gizmos.DrawCube(areaCenter, areaSize);
            }
        }
    }
}
