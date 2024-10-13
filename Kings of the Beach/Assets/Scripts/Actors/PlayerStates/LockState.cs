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
            player.BallHitGround += OnBallHitGround;
        }

        public override void Exit() {
            player.InputReader.bumpEvent -= OnPass;
            player.InputReader.bumpAcrossEvent -= OnBumpAcross;
            player.BallHitGround -= OnBallHitGround;
        }

        public override void Update() {
            bumpTimer -= Time.deltaTime;
            TryUnlock();
        }

        public override void OnTriggerEnter(Collider other) {
            if (player.Ball != null) {
                if (!player.IsJumping) {
                    if (bumpTimer > 0) {
                        player.Pass(targetPos);
                        canUnlock = true;
                        unlockTimer = unlockDelay;
                    }
                } else {
                    // shot
                    SetTargetPos(false);
                    player.BallInfo.SetSpikeTarget(targetPos, Random.Range(0.5f, 1f), player);
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

        private void OnTargetMoved() {
            // go to NormalState
        }

        private void OnBallHitGround() {
            player.StateMachine.ChangeState(player.PostPointState);
        }
    }
}
