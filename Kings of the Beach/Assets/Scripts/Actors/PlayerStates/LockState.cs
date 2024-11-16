using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class LockState : PlayerBaseState
    {
        public LockState(Player player) : base(player) { }

        private float bumpTimer;
        private Vector3 targetPos;
        protected bool canUnlock;
        protected float unlockTimer;
        private float unlockDelay = 0.25f;

        public override void Enter() {
            bumpTimer = 0;
            canUnlock = false;

            player.InputReader.bumpEvent += OnPass;
            player.InputReader.bumpAcrossEvent += OnBumpAcross;
        }

        public override void Exit() {
            player.InputReader.bumpEvent -= OnPass;
            player.InputReader.bumpAcrossEvent -= OnBumpAcross;
        }

        public override void Update() {
            bumpTimer -= Time.deltaTime;
            TryUnlock();
        }

        public override void OnTriggerEnter(Collider other) {
            if (player.Ball != null) {
                if (!player.IsJumping) {
                    if (bumpTimer > 0) {
                        player.Pass(targetPos, 7, 1.75f);
                        canUnlock = true;
                        unlockTimer = unlockDelay;
                    }
                } else {
                    SetTargetPos(false);
                    if (!player.Feint) {
                        Animator animator = player.GetComponentInChildren<Animator>();
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        float kingFrame = 7;
                        float totalFrames = 18;
                        float perfectContact = kingFrame / totalFrames;
                        Debug.Log($"contact: {stateInfo.normalizedTime} perfect: {perfectContact} result: {stateInfo.normalizedTime / perfectContact}");
                        player.Spike(targetPos);
                    } else {
                        player.Pass(targetPos, 5, 1);
                    }
                    player.StateMachine.ChangeState(player.NormalState);
                }
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
                    player.StateMachine.ChangeState(player.NormalState);
                }
            }
        }

        private void OnPass() {
            Bump(true);
        }

        private void OnBumpAcross() {
            Bump(false);
        }
    }
}
