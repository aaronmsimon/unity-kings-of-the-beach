using System;
using UnityEngine;
using KotB.Actors;

namespace KotB.Items
{
    [CreateAssetMenu(fileName = "BallInfo", menuName = "Game/Ball Info")]
    public class BallSO : ScriptableObject
    {
        [Header("Skill Values")]
        [SerializeField] private SkillValues skillValues;

        public Vector3 Position { get; set; }
        [Header("Everything else...")]
        [SerializeField] private Vector3 targetPos;
        public Vector3 StartPos { get; private set; }
        public float Height { get; set; }
        public float Duration { get; set; }
        [SerializeField] private int possession;
        [SerializeField] private int hitsForTeam;
        public Athlete lastPlayerToHit { get; set; }
        public float TimeSinceLastHit { get; set; }
        public float BallRadius { get; set; }
        public bool LockedOn;

        public Athlete ballHeldBy { get; private set; }

        public event Action BallGiven;
        public event Action TargetSet;
        public event Action BallServed;

        // Serve
        private float idealServeHeight = 3f;
        private float minServeHeight = 2f;
        private float maxServeHeight = 5f;
        private float minServeDistance = 7f;
        private float maxServeDistance = 18f;

        private float netHeight = 2.43f;
        
        public void GiveBall(Athlete athlete) {
            ballHeldBy = athlete;
            BallGiven?.Invoke();
        }

        public bool ValidServe(Vector3 aimPoint, float servePower, Athlete server) {
            Height = aimPoint.y;
            Height = Mathf.Clamp(Height, minServeHeight, maxServeHeight);

            // Calculate the parabolic scaling factor
            float normalizedHeight = (Height - idealServeHeight) / (maxServeHeight - minServeHeight);
            float parabolicFactor = 1f - Mathf.Clamp01(normalizedHeight * normalizedHeight);

            // Calculate the new distance based on the parabolic relationship
            float possibleDistance = minServeDistance + (maxServeDistance - minServeDistance) * parabolicFactor;
            float serveDistance = possibleDistance * servePower;

            // Calculate Target Position
            Vector3 direction = new Vector3(aimPoint.x, 0, aimPoint.z) - new Vector3(Position.x, 0, Position.z);
            direction.Normalize();
            TargetPos = new Vector3(Position.x, 0, Position.z) + direction * serveDistance;

            StartPos = Position;
            float serveSpeed = skillValues.SkillToValue(server.Skills.ServePower, skillValues.ServePower) * servePower;
            Duration = serveDistance / serveSpeed / 1000 * 60 * 60; // d x 1/r x 1km/1000m x 60min/hr x 60sec/min = sec
            // Debug.Log($"Distance: {serveDistance} Duration: {Duration} Serve Speed: {Mathf.Round(serveDistance / Duration / 1000 * 60 * 60 * 10) / 10} km/hr");
            lastPlayerToHit = server;

            float netXPosCheck = 0.13f * server.CourtSide;
            Vector3 ballAtNetPos = PredictHeightAtX(netXPosCheck, StartPos, TargetPos, Height);
            // Debug.Log($"PredictHeightAtX({netXPosCheck}, {StartPos}, {TargetPos}, {Height}) -> Estimate serve height: {ballAtNetPos.y}");

            return MathF.Abs(ballAtNetPos.z) <= server.CourtSideLength / 2 && ballAtNetPos.y > netHeight;
        }

        public void SetServeTarget() {
            ResetTimeSinceLastHit();
            TargetSet?.Invoke();
            BallServed?.Invoke();
        }

        public void SetPassTarget(Vector3 targetPos, float height, float duration, Athlete passer) {
            StartPos = Position;
            TargetPos = targetPos;
            Height = height;
            Duration = duration;
            ResetTimeSinceLastHit();
            HitsForTeam += 1;
            lastPlayerToHit = passer;
            TargetSet?.Invoke();
        }

        public void SetSpikeTarget(Vector3 targetPos, float duration, Athlete spiker) {
            StartPos = Position;
            TargetPos = targetPos;
            Height = -1;
            Duration = duration;
            ResetTimeSinceLastHit();
            HitsForTeam += 1;
            lastPlayerToHit = spiker;
            TargetSet?.Invoke();
        }

        public void ClearTargetPos() {
            targetPos = Vector3.zero;
        }

        public bool IsInBounds(Vector3 posToCheck) {
            // sorry for magic numbers
            return
                (posToCheck.x + BallRadius) >= -8 &&
                (posToCheck.x - BallRadius) <= 8 &&
                (posToCheck.z + BallRadius) >= -4 &&
                (posToCheck.z - BallRadius) <= 4;
        }

        public Vector3 CalculateInFlightPosition(float t, Vector3 start, Vector3 end, float maxHeightPoint)
        {
            // Linear interpolation for horizontal position (XZ plane)
            Vector3 horizontalPosition = Vector3.Lerp(start, end, t);

            // Parabolic interpolation for vertical position (Y-axis)
                // Using quadratic Bezier curve formula
                float oneMinusT = 1f - t;
                
                // Calculate control point for quadratic curve to pass through maxHeightPoint at t=0.5
                float controlPoint = (maxHeightPoint - (0.25f * start.y) - (0.25f * end.y)) / 0.5f;
                
                // Quadratic interpolation
                float verticalPosition = (oneMinusT * oneMinusT * start.y) + 
                    (2f * oneMinusT * t * controlPoint) + 
                    (t * t * end.y);

            // Combine horizontal and vertical movement
            horizontalPosition.y = verticalPosition;

            return horizontalPosition;
        }

        public Vector3 CalculateInFlightPosition(float t, Vector3 start, Vector3 end) {
            // Linear interpolation for horizontal position (XZ plane)
            return Vector3.Lerp(start, end, t);
        }

        private void ResetTimeSinceLastHit() {
            LockedOn = false;
            TimeSinceLastHit = 0;
        }
        
        private Vector3 PredictHeightAtX(float targetX, Vector3 start, Vector3 end, float maxHeightPoint)
        {
            // First ensure the ball actually crosses this X position
            if (!((start.x <= targetX && end.x >= targetX) || (start.x >= targetX && end.x <= targetX)))
            {
                return Vector3.zero; // or some error value
            }

            // Since X movement is linear, we can find exact t when we hit targetX
            float t = Mathf.Abs((targetX - start.x) / (end.x - start.x));
            
            // Now we can calculate the exact position at this time t using your original flight calculation
            return CalculateInFlightPosition(t, start, end, maxHeightPoint);
        }

        //---- PROPERTIES ----
        public int Possession { get { return possession; } set { possession = value; } }
        public Vector3 TargetPos { get { return targetPos; } set { targetPos = value; } }
        public SkillValues SkillValues { get { return skillValues; } }
        public int HitsForTeam { get { return hitsForTeam; } set { hitsForTeam = value; } }
    }
}
