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

        public override void Update()
        {
            bumpTimer -= Time.deltaTime;
            TryUnlock();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (bumpTimer > 0) {
                player.BallInfo.SetPassTarget(targetPos, 7, 1.75f);
                player.BallInfo.HitsForTeam += 1;
                player.BallInfo.lastPlayerToHit = player;
                canUnlock = true;
                unlockTimer = unlockDelay;
            }
        }

        private void Bump(bool pass) {
            bumpTimer = player.CoyoteTime;

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
