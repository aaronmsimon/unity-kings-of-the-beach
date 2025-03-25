using UnityEngine;
using KotB.Actors;
using KotB.Items;

namespace KotB.StatePattern.AIStates
{
    public class DefenseBlockerState : AIBaseState
    {
        public DefenseBlockerState(AI ai) : base(ai) { }
        
        private Vector3 targetPos;
        private float reactionTime;
        private float spikeTime;
        private bool isBlocking;

        private float blockPos = 1;

        public override void Enter() {
            targetPos = ai.transform.position;
            reactionTime = ai.Skills.ReactionTime;
            ai.FaceOpponent();
            
            isBlocking = false;
            
            ai.BallInfo.BallPassed += OnBallPassed;
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.BallPassed -= OnBallPassed;
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ai.TargetPos = targetPos;

            if (Mathf.Sign(ai.BallInfo.Position.x) != ai.CourtSide && ai.BallInfo.HitsForTeam == 2 && !isBlocking) {
                AnticipateSpike();
            }

            // Instead of using the OnTriggerEnter
            if (ai.IsJumping) {
                // Check for distance to overlapsphere
                float locscale = ai.transform.localScale.x;
                Vector3 manualPos = ai.transform.position + ai.SpikeBlockCollider.center;
                Debug.Log($"manual collider distance: {DistanceToSphere(ai.BallInfo.Position, manualPos + Vector3.up * manualPos.y * (locscale - 1), ai.SpikeBlockCollider.radius * locscale)}");

                // Check for collision manually
                Collider[] collisions = Physics.OverlapSphere(
                    ai.SpikeBlockCollider.transform.position + ai.SpikeBlockCollider.center, 
                    ai.SpikeBlockCollider.radius);
                    
                foreach (var col in collisions) {
                    if (col.TryGetComponent<Ball>(out Ball ball)) {
                        ai.BlockAttempt();
                        break;
                    }
                }
            }
        }

        public override void OnTriggerEnter(Collider other) {
            if (ai.Ball != null) {
                ai.BlockAttempt();
            }
        }

        private void OnTargetSet() {
            // Need to add consideration if a shot is not blockable
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) != ai.CourtSide) {
                targetPos = new Vector3(blockPos * ai.CourtSide, ai.transform.position.y, ai.BallInfo.TargetPos.z);
            } else {
                if (ai.BallInfo.Possession == ai.CourtSide) {
                    ai.StateMachine.ChangeState(ai.DigReadyState);
                }
            }
        }

        private void AnticipateSpike() {
            float jumpDuration = ai.JumpFrames / ai.AnimationFrameRate;
            // Debug.Log($"{ai.BallInfo.TimeSinceLastHit} >= {spikeTime} - {jumpDuration} - {reactionTime} && {spikeTime} >= 0");
            if (ai.BallInfo.TimeSinceLastHit >= spikeTime - jumpDuration - reactionTime && spikeTime >= 0) {
                Debug.Log($"{ai.Skills.AthleteName} jumping to try to block now: {ai.BallInfo.TimeSinceLastHit} >= {spikeTime} - {jumpDuration} - {reactionTime}");
                ai.PerformJump();
                isBlocking = true;
            }
        }

        private void OnBallPassed() {
            float optimalSpikeHeight = 4;
            spikeTime = ai.GetTimeToContactHeight(optimalSpikeHeight, ai.BallInfo.Height, ai.BallInfo.StartPos.y, ai.BallInfo.TargetPos.y, ai.BallInfo.Duration);
            Debug.Log($"{ai.Skills.AthleteName} is estimating spike time from {optimalSpikeHeight}, {ai.BallInfo.Height}, {ai.BallInfo.StartPos.y}, {ai.BallInfo.TargetPos.y}, {ai.BallInfo.Duration}");
        }

// TEMPORARY
private float DistanceToSphere(Vector3 point, Vector3 sphereCenter, float sphereRadius)
{
    // Calculate the distance between the point and sphere center
    float distance = Vector3.Distance(point, sphereCenter);
    
    // If the point is inside the sphere, return 0 (or a negative value if you prefer)
    if (distance <= sphereRadius)
        return 0f; // Point is inside or on the sphere
    
    // Otherwise, return the distance to the sphere surface
    return distance - sphereRadius;
}
    }
}
