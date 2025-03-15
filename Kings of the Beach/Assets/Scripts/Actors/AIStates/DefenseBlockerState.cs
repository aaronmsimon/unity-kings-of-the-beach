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
                ai.StateMachine.ChangeState(ai.OffenseState);
            } else {
                targetPos = new Vector3(blockPos * ai.CourtSide, ai.transform.position.y, ai.BallInfo.TargetPos.z);
                AnticipateSpike();
            }
        }

        private void AnticipateSpike() {
            // Calculate how long until an opponent would likely spike
            float distanceToNet = Mathf.Abs(ai.BallInfo.TargetPos.x);
            
            // Find the opposing team's attacker
            Athlete attacker = null;
            foreach (Athlete athlete in ai.MatchInfo.GetOpposingTeam(ai).Athletes) {
                // If we can identify the likely spiker (closest to ball, offensive position, etc.)
                if (attacker == null || 
                    Vector3.Distance(athlete.transform.position, ai.BallInfo.TargetPos) < 
                    Vector3.Distance(attacker.transform.position, ai.BallInfo.TargetPos)) {
                    attacker = athlete;
                }
            }
            
            // Base anticipation timing - how long after the setup an attacker typically spikes
            float baseAnticipationTime = 0.6f; // Default timing
            
            if (attacker != null) {
                // Adjust based on opponent's spike skill - better spikers hit quicker
                float attackerSkillFactor = ai.BallInfo.SkillValues.SkillToValue(
                    attacker.Skills.SpikeSkill, 
                    new MinMax(0.8f, 0.5f) // Better spikers hit faster after setup
                );
                
                // Adjust based on pass height - higher passes give more time
                float heightFactor = ai.BallInfo.Height / optimalSpikeHeight;
                heightFactor = Mathf.Clamp(heightFactor, 0.8f, 1.5f);
                
                // Calculate anticipation time
                baseAnticipationTime *= attackerSkillFactor * heightFactor;
            }
            
            // Adjust blocker timing based on skill
            float blockerReactionSkill = ai.BallInfo.SkillValues.SkillToValue(
                ai.Skills.Blocking, 
                new MinMax(0.3f, -0.1f) // Skill range: high skill might even anticipate (-0.1), low skill is late (0.3)
            );
            
            // Jump animation time
            float jumpAnimationTime = ai.JumpFrames / ai.AnimationFrameRate;
            
            // Final calculation of when to jump
            anticipationTimer = baseAnticipationTime + blockerReactionSkill - (jumpAnimationTime * 0.5f);
            
            // Add some randomization based on blocker skill
            float randomVariation = Random.Range(-0.1f, 0.1f) * (11 - ai.Skills.Blocking) / 10f;
            anticipationTimer += randomVariation;
            
            // Debug.Log($"Anticipating spike in {baseAnticipationTime}s, jumping in {anticipationTimer}s (skill: {blockerReactionSkill}, anim: {jumpAnimationTime})");
            
            readyToJump = true;
        }
    }
}
