using UnityEngine;
using Cackenballz.Helpers;

namespace KotB.Actors
{
    public class Teammate : Athlete
    {
        private enum AIState {
            DigReady,
            Offense,
            Defense,
            Service
        }

        [Header("Behavior")]
        [SerializeField][Range(0,1)] private float mySidePct = 0.5f;
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

        protected override void Start() {
            // setting state on start isn't safe since ball isn't instantiated yet - need to use an event (hard-coding for now)
            if (this.name == "Teammate") {
                state = AIState.DigReady;
            }
            else {
                state = AIState.Service;
            }
            base.Start();
        }

        protected override void Update() {
            UpdateMyZone();

            DetermineMovement();

            base.Update();
        }

        protected override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
            bumpTimer = Mathf.Infinity; // any positive value to trigger the bump
            if (ballInfo.HitsForTeam < 2) {
                // pass
                Vector2 teammatePos = new Vector2(teammateSO.Position.x, teammateSO.Position.z);
                Vector2 aimLocation = AdjustVectorAccuracy(teammatePos, skills.PassAccuracy);
                bumpTarget = new Vector3(aimLocation.x, 0f, aimLocation.y);
            } else {
                // shot
                bumpTarget = new Vector3(Random.Range(0, courtSideLength * -courtSide), 0f, Random.Range(-courtSideLength / 2, courtSideLength / 2));
            }
        }

        private void DetermineMovement() {
            if (ballInfo == null) return;
            if (ballInfo.ballState != BallState.Bump) return;

            Vector2 ballTarget = new Vector2(ballInfo.Target.x, ballInfo.Target.z);
            moveDir = Vector3.zero;

            switch (state) {
                case AIState.DigReady:
                    if (IsTargetInMyZone(ballTarget)) {
                        moveDir = (ballInfo.Target - transform.position).normalized;
                    } else {
                        // move to offensive position
                    }
                    if (Mathf.Sign(ballInfo.Target.x) == courtSide && ballInfo.HitsForTeam == 1) {
                        state = AIState.Offense;
                    }
                    break;
                case AIState.Offense:
                    if (ballInfo.lastPlayerToHit != this && ballInfo.lastPlayerToHit != null) {
                        moveDir = (ballInfo.Target - transform.position).normalized;
                    } else {
                        // move to offensive position
                    }
                    if (Mathf.Sign(ballInfo.Target.x) == -courtSide) {
                        state = AIState.Defense;
                    }
                    break;
                case AIState.Defense:
                    // move to defensive position
                    if (Mathf.Sign(ballInfo.Target.x) == -courtSide) {
                        state = AIState.DigReady;
                    }
                    break;
                case AIState.Service:
                    // move to defensive position
                    // when service completed
                    state = AIState.Defense;
                    break;
                default:
                    Debug.LogError("AI State not handled.");
                    break;
            }
        }

        private void UpdateMyZone() {
            // If on the correct side
            if (Mathf.Sign(transform.position.x) == courtSide) {
                // Check if at net
                if (Mathf.Abs(transform.position.x) <= skills.MyBlockArea) {
                    myZoneTopLeft = new Vector2(courtSide * (skills.MyBlockArea / 2) + (skills.MyBlockArea / -2), courtSideLength / 2);
                    myZoneBotRight = new Vector2(courtSide * (skills.MyBlockArea / 2) + (skills.MyBlockArea / 2), -courtSideLength / 2);
                }
                // Check if teammate at net
                else if (Mathf.Abs(teammateSO.Position.x) <= teammateSO.MyBlockArea) {
                    myZoneTopLeft = new Vector2(transform.position.x > 0 ? teammateSO.MyBlockArea / 2 + teammateSO.MyBlockArea / -2 + teammateSO.MyBlockArea : -courtSideLength, courtSideLength / 2);
                    myZoneBotRight = new Vector2(transform.position.x > 0 ? courtSideLength : -teammateSO.MyBlockArea, -courtSideLength / 2);
                }
                // If not near front nor back, then take their side
                else {
                        float startYpos = transform.position.z > 0 ? courtSideLength / 2 : 4 - (courtSideLength * (1 - mySidePct));
                        float halfCourtSideLength = courtSideLength / 2;
                        myZoneTopLeft = new Vector2(courtSide * halfCourtSideLength - halfCourtSideLength, startYpos);
                        myZoneBotRight = new Vector2(courtSide * halfCourtSideLength + halfCourtSideLength, startYpos - (courtSideLength * mySidePct));
                }
            } else {
                myZoneTopLeft = Vector2.zero;
                myZoneBotRight = Vector2.zero;
            }
        }

        private bool IsTargetInMyZone(Vector2 point)
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
            
            // Dig Range
            GizmoHelpers.DrawGizmoCircle(transform.position, skills.DigRange, Color.red, 12);
        }
    }
}
