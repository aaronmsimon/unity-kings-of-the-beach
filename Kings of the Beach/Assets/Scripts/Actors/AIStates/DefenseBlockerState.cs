using UnityEngine;
using KotB.Actors;

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
        private bool lastEnabledStatus = false;

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

            if (ai.BallInfo.HitsForTeam == 2 && !isBlocking) {
                AnticipateSpike();
            }
            if (ai.SpikeBlockCollider.enabled != lastEnabledStatus) {
                lastEnabledStatus = ai.SpikeBlockCollider.enabled;
                Debug.Log($"{ai.Skills.AthleteName} changed collider enabled to {ai.SpikeBlockCollider.enabled} with time since last hit of {ai.BallInfo.TimeSinceLastHit}");
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
                // ai.StateMachine.ChangeState(ai.OffenseState);
            } else {
                targetPos = new Vector3(blockPos * ai.CourtSide, ai.transform.position.y, ai.BallInfo.TargetPos.z);
            }
        }

        private void AnticipateSpike() {
            float jumpDuration = ai.JumpFrames / ai.AnimationFrameRate;
            float spikeDuration = ai.ActionFrames / ai.AnimationFrameRate;
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
    }
}
