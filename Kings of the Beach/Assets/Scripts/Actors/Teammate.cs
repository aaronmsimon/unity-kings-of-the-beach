using Unity.VisualScripting;
using UnityEngine;

namespace KotB.Actors
{
    public class Teammate : Athlete
    {
        [Header("Behavior")]
        [SerializeField][Range(0,1)] private float mySide = 0.5f;
        [SerializeField] private bool showMySide;
        [SerializeField] private Color mySideColor;

        private float squareLength = 8;

        protected override void Update() {
            if (ballSO == null) return;

            Vector2 ballTarget = new Vector2(ballSO.Target.x, ballSO.Target.z);

            // Serve: only receive if in zone
            if (ballSO.ballState == BallState.Bump && IsPointOnTeamSide(ballTarget)) {
                if (ballSO.HitsForTeam == 0 && IsPointWithinMySide(ballTarget)) {
                    moveDir = (ballSO.Target - transform.position).normalized;
                }
                // Any other hit: will hit if it's their turn
                else if(ballSO.lastPlayerToHit != this) {
                    moveDir = (ballSO.Target - transform.position).normalized;
                }
                else {
                    moveDir = Vector3.zero;
                }
            }

            base.Update(); 
        }

        protected override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
            bumpTimer = 5; // any positive value to trigger the bump
            float targetX;
            if (ballSO.HitsForTeam < 2) {
                targetX = Random.Range(-8, 0);
            } else {
                targetX = Random.Range(0, 8);
            }
            float targetZ = Random.Range(-4, 4);
            bumpTarget = new Vector3(targetX, 0f, targetZ);
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
            return point.x < 0;
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
