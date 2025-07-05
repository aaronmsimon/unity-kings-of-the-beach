using UnityEngine;
using KotB.Actors;
using KotB.Items;

namespace KotB.Testing
{
    public class AIBumpSet : AI
    {
        [SerializeField] private PassType passType;

        private float reactionTime;

        protected override void Start() {
            base.Start();

            TargetPos = transform.position;
            reactionTime = skills.ReactionTime;
        //     spikeTime = GetTimeToContactHeight(ReachHeight, BallInfo.Height, BallInfo.StartPos.y, BallInfo.TargetPos.y, BallInfo.Duration);
            FaceOpponent();
            Reset();
        }

        private void OnEnable() {
            AnimBumpSetTrigger.Triggered += OnAnimTriggered;
            BodyTrigger.Triggered += OnBodyTriggered;
        }

        private void OnDisable() {
            AnimBumpSetTrigger.Triggered -= OnAnimTriggered;
            BodyTrigger.Triggered -= OnBodyTriggered;
        }

        protected override void Update() {
            base.Update();

            reactionTime -= Time.deltaTime;

            if (reactionTime < 0) {
                if ((transform.position - BallInfo.TargetPos).sqrMagnitude > Skills.TargetLockDistance * Skills.TargetLockDistance && !BallInfo.LockedOn && ballInfo.LastPlayerToHit != this) {
                    Vector2 lockTowardsTarget = LockTowardsTarget();
                    Vector3 lockPos = new Vector3(lockTowardsTarget.x, 0.01f, lockTowardsTarget.y);
                    TargetPos = lockPos;
                    BallInfo.LockedOn = true;
                // } else {
        //             if (BallInfo.HitsForTeam == 2 && !isSpiking) {
        //                 TrySpike();
        //             }
                }
            }
        //     Debug.Log($"Locked on: {ballInfo.LockedOn}");
        }

        public void Reset() {
            AnimBumpSetTrigger.Active = true;
        }

        private void OnAnimTriggered(Collider other) {
            PlayAnimation(passType.ToString());
        }

        private void OnBodyTriggered(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                Pass(new Vector3(-8, 0.01f, 4), 7, 1.75f, passType);
            }
        }
    }
}
