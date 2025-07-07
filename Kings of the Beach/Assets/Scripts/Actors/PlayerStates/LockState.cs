using UnityEngine;
using KotB.Actors;
using KotB.Items;

namespace KotB.StatePattern.PlayerStates
{
    public class LockState : PlayerBaseState
    {
        public LockState(Player player) : base(player) { }

        private float bumpTimer;
        private Vector3 targetPos;
        private bool canUnlock;
        private float unlockTimer;
        private float unlockDelay = 0.25f;
        private float spikeWindowPenalty = 10;
        private Animator animator;
        private AnimatorStateInfo stateInfo;
        private PassType passType;

        public override void Enter() {
            bumpTimer = 0;
            canUnlock = false;

            animator = player.GetComponentInChildren<Animator>();
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            player.BodyTrigger.Triggered += OnBodyTriggered;
            player.SpikeTrigger.Triggered += OnSpikeTriggered;
            player.InputReader.bumpEvent += OnBump;
            player.InputReader.bumpAcrossEvent += OnBumpAcross;
            player.InputReader.setEvent += OnSet;
            player.BallInfo.TargetSet += OnTargetSet;
        }

        public override void Exit() {
            player.BodyTrigger.Triggered -= OnBodyTriggered;
            player.SpikeTrigger.Triggered -= OnSpikeTriggered;
            player.InputReader.bumpEvent -= OnBump;
            player.InputReader.bumpAcrossEvent -= OnBumpAcross;
            player.InputReader.setEvent -= OnSet;
            player.BallInfo.TargetSet -= OnTargetSet;
        }

        public override void Update() {
            bumpTimer -= Time.deltaTime;
            TryUnlock();
        }

        private void OnBodyTriggered(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                if (bumpTimer > 0) {
                    player.Pass(targetPos, 7, 1.75f, passType);
                    unlockTimer = unlockDelay;
                }
            }
        }

        private void OnSpikeTriggered(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                SetTargetPos(false);
                if (!player.Feint) {
                    float timingVar = stateInfo.normalizedTime - 1;
                    float window = player.BallInfo.SkillValues.SkillToValue(player.Skills.SpikeSkill, player.BallInfo.SkillValues.SpikeTimingWindow);
                    float penalty = timingVar * window * spikeWindowPenalty;
                    player.SpikeSpeedPenalty = timingVar * window;
                    Vector3 newTargetPos = new Vector3(targetPos.x + penalty, targetPos.y, targetPos.z);
                    // Debug.Log($"timingVar: {timingVar} window: {window} penalty: {penalty} target: {targetPos} newtarget: {newTargetPos}");
                    player.Spike(newTargetPos);
                } else {
                    player.SpikeFeint(targetPos);
                }
                player.UnlockPlayer();
            }
        }

        private void Bump(bool pass) {
            bumpTimer = player.CoyoteTime;
            SetTargetPos(pass);
        }

        private void SetTargetPos(bool pass) {
            Vector2 aim = Helpers.CircleMappedToSquare(player.MoveInput.x, player.MoveInput.y);

            float targetX = aim.x * 5 + 4 * (pass ? player.CourtSide : -player.CourtSide);
            float targetZ = aim.y * 5;
            targetPos = new Vector3(targetX, 0f, targetZ);
        }
        
        private void TryUnlock() {
            if (canUnlock) {
                unlockTimer -= Time.deltaTime;
                if (unlockTimer <= 0) {
                    player.UnlockPlayer();
                }
            }
        }

        private void OnBump() {
            passType = PassType.Bump;
            Bump(true);
        }

        private void OnBumpAcross() {
            passType = PassType.Bump;
            Bump(false);
        }

        private void OnSet() {
            passType = PassType.Set;
            Bump(true);
        }

        private void OnTargetSet() {
            canUnlock = true;
        }
    }
}
