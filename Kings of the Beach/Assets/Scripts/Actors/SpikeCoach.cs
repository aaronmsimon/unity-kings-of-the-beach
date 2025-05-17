using System;
using UnityEngine;
using KotB.Match;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Actors
{
    public class SpikeCoach : AI
    {
        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private Vector2 targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor;

        [Header("Game Input")]
        [SerializeField] private InputReader inputReader;

        [Header("Game Events")]
        [SerializeField] private GameEvent resetBallEvent;
        [SerializeField][Range(0,2)] private int resetHitCounterAmount;

        // public event Action BallTaken;

        private MatchManager matchManager;

        private Vector3 startPos;
        private bool isSpiking;
        private float spikeTime;
        
        protected override void Start()
        {
            base.Start();

            startPos = transform.position;
            isSpiking = false;
            spikeTime = GetTimeToContactHeight(ReachHeight, BallInfo.Height, BallInfo.StartPos.y, BallInfo.TargetPos.y, BallInfo.Duration);
        }

        protected override void Update() {
            if ((transform.position - BallInfo.TargetPos).sqrMagnitude > Skills.TargetLockDistance * Skills.TargetLockDistance && !BallInfo.LockedOn) {
                Vector2 lockTowardsTarget = LockTowardsTarget();
                Vector3 lockPos = new Vector3(lockTowardsTarget.x, 0.01f, lockTowardsTarget.y);
                TargetPos = lockPos;
                BallInfo.LockedOn = true;
            } else {
                if (!isSpiking) {
                    TrySpike();
                }
            }
            
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TargetPos = startPos;
            }
            #endif
        }

        private void TrySpike() {
            float jumpDuration = JumpFrames / AnimationFrameRate;
            float spikeDuration = ActionFrames / AnimationFrameRate;
            if (BallInfo.TimeSinceLastHit >= spikeTime - jumpDuration - spikeDuration / 2 && spikeTime >= 0) {
                PerformJump();
                isSpiking = true;
            }
        }

        //---- EVENT LISTENERS ----

        //---- GIZMOS ----

        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();

            Helpers.DrawTargetZone(targetZonePos, targetZoneSize, targetZoneColor, showTargetZone);
        }
    }
}
