using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class AIBlocker : AI
    {
        [SerializeField] private Player player;

        private Vector3 tgt;
        private float reactionTime;
        private float spikeTime;
        private Vector3 spikePosEstimate;

        private bool isBlocking = false;
        private float blockPos = 1;

        protected override void Start() {
            base.Start();

            tgt = transform.position;
            reactionTime = skills.ReactionTime;
            FaceOpponent();
            Reset();
        }

        private void OnEnable() {
            ballInfo.TargetSet += OnTargetSet;
            ballInfo.BallPassed += OnBallPassed;
        }

        private void OnDisable() {
            ballInfo.TargetSet -= OnTargetSet;
            ballInfo.BallPassed -= OnBallPassed;
        }

        protected override void Update() {
            base.Update();
            TargetPos = tgt;

            if (Mathf.Sign(ballInfo.Position.x) != courtSide.Value && ballInfo.HitsForTeam == 2 && !isBlocking) {
                AnticipateSpike();
            }
        }

        public void Reset() {
            ballInfo.HitsForTeam = 1;
            isBlocking = false;
            OnJumpCompletedEvent();
        }

        private void AnticipateSpike() {
            float jumpDuration = JumpFrames / AnimationFrameRate;
            float guessedSpeed = ballInfo.SkillValues.SkillToValue(player.Skills.SpikePower, ballInfo.SkillValues.SpikePower);
            float spikeToBlockDist = Vector3.Distance(spikePosEstimate, transform.position);
            float estimatedTimeToBlock = spikeToBlockDist / guessedSpeed;
            if (ballInfo.TimeSinceLastHit >= spikeTime + estimatedTimeToBlock - jumpDuration - reactionTime && spikeTime >= 0) {
                PerformJump();
                isBlocking = true;
            }
        }

        private void OnTargetSet() {
            if (Mathf.Sign(ballInfo.TargetPos.x) != courtSide.Value) {
                tgt = new Vector3(blockPos * courtSide.Value, transform.position.y, ballInfo.TargetPos.z);
                Debug.Log($"target is now: {tgt}");
            }
        }

        private void OnBallPassed() {
            float optimalSpikeHeight = 4;
            spikeTime = GetTimeToContactHeight(optimalSpikeHeight, ballInfo.Height, ballInfo.StartPos.y, ballInfo.TargetPos.y, ballInfo.Duration);
            spikePosEstimate = ballInfo.TargetPos;
        }
    }
}
