using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.AIStates
{
    public class DefenseBlockerState : AIBaseState
    {
        public DefenseBlockerState(AI ai) : base(ai) { }
        
        private Vector3 targetPos;

        private float blockPos = 1;
        private float jumpTimeRemaining = -1;
        private bool readyToJump = false;

        public override void Enter() {
            targetPos = ai.transform.position;
            ai.FaceOpponent();
            
            ai.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            ai.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            ai.TargetPos = targetPos;

            // If we're ready to anticipate a jump
            if (readyToJump) {
                jumpTimeRemaining -= Time.deltaTime;
                Debug.Log($"jumpTimeRemaining: {jumpTimeRemaining}");
                
                // Time to jump!
                if (jumpTimeRemaining <= 0) {
                    ai.PerformJump();
                    ai.StateMachine.ChangeState(ai.OffenseState);
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
            if (Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide) {
                CalculateBlockTiming();
            } else {
                targetPos = new Vector3(blockPos * ai.CourtSide, ai.transform.position.y, ai.BallInfo.TargetPos.z);
            }
        }

        private void CalculateBlockTiming() {
            // Predict when ball will reach block position
            Vector3 ballStart = ai.BallInfo.StartPos;
            Vector3 ballTarget = ai.BallInfo.TargetPos;
            float totalBallTime = ai.BallInfo.Duration;
            
            // Calculate time for ball to reach block position
            float blockX = ai.transform.position.x;
            
            // Calculate time when ball reaches block position
            // This uses linear interpolation of x position to estimate time
            float blockPositionRatio = Mathf.Abs((blockX - ballStart.x) / (ballTarget.x - ballStart.x));
            
            float timeToBlock = blockPositionRatio * totalBallTime;
            
            // Calculate when to jump based on:
            // 1. Time for AI to react (skill based)
            // 2. Time for jump animation to reach apex
            float jumpAnimationTime = ai.JumpFrames / ai.AnimationFrameRate;
            // float reactionDelay = ai.BallInfo.SkillValues.SkillToValue(
            //     ai.Skills.Blocking, 
            //     new MinMax(0.2f, 0f) // Better blocking skill = less reaction delay
            // );
            
            // Time until we need to start the jump
            jumpTimeRemaining = timeToBlock - jumpAnimationTime/*2 - reactionDelay*/;
            
            // Add some blocking skill-based randomization
            // float randomFactor = Random.Range(-0.1f, 0.1f) * (11 - ai.Skills.Blocking) / 10f;
            // jumpTimeRemaining += randomFactor;
            
            // Only jump if there's actually time to do so
            readyToJump = jumpTimeRemaining > 0;
            
            Debug.Log($"Ball is starting at {ballStart} with a target of {ballTarget}. The blocker is at x={blockX} so the the ball will be in a blocking position at " +
                $"{blockPositionRatio:F2} of the total time ({totalBallTime}), or {timeToBlock}. With an animation time of {jumpAnimationTime}, the athlete will need to jump in {jumpTimeRemaining}.");
        }
    }
}
