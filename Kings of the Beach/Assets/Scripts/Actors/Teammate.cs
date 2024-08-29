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
            base.Update(); 

            if (ballSO == null) return;

            Vector2 ballTarget = new Vector2(ballSO.Target.x, ballSO.Target.z);
            if (ballSO.ballState == BallState.Bump && IsPointWithinMySide(ballTarget)) {
                moveInput = ballTarget;
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

        private void OnDrawGizmos() {
            if (showMySide) {
                Vector2 mySideArea = new Vector2(squareLength, squareLength * mySide);
                Vector3 areaCenter = new Vector3(-mySideArea.x / 2, 0, squareLength / 2 - mySideArea.y / 2);
                Vector3 areaSize = new Vector3(mySideArea.x, 0.1f, mySideArea.y);

                Gizmos.color = mySideColor;
                Gizmos.DrawCube(areaCenter, areaSize);
            }
        }

        //---- EVENT LISTENERS ----
        public void OnTargetSet() {

        }
    }
}
