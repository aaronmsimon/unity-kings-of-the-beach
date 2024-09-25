using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class LockState : PlayerBaseState
    {
        public LockState(Player player) : base(player) { }

        public override void Enter() {
            Debug.Log("Entering the Player Lock state.");
            player.InputReader.bumpEvent += OnBump;
            player.InputReader.bumpAcrossEvent += OnBumpAcross;
        }

        public override void Exit() {
            player.InputReader.bumpEvent -= OnBump;
            player.InputReader.bumpAcrossEvent -= OnBumpAcross;
            Debug.Log("Exiting the Player Lock State.");
        }

        public override void Update()
        {
            player.Bump();
            TryUnlock();
        }

        // private void Bump(bool pass) {
        //     bumpTimer = coyoteTime;

        //     Vector2 aim = CircleMappedToSquare(moveInput.x, moveInput.y);

        //     float targetX = aim.x * 5 + 4 * (pass ? courtSide : -courtSide);
        //     float targetZ = aim.y * 5;
        //     bumpTarget = new Vector3(targetX, 0f, targetZ);
        // }
        
        // <athlete>
        // the logic for the timer (coyote time) should only be handled by the player
        // public void Bump() {
        //     bumpTimer -= Time.deltaTime;
        //     if (canBump && bumpTimer > 0 && this.ball != null) {
        //         this.ball.Bump(bumpTarget, 12, 2);
        //         canUnlock = true;
        //         unlockTimer = unlockDelay;
        //         canBump = false;
        //         ballInfo.HitsForTeam += 1;
        //         Debug.Log("Hits: " + ballInfo.HitsForTeam);
        //         ballInfo.lastPlayerToHit = this;
        //     }
        // }
        // </athlete>

        private void TryUnlock() {
            // if (canUnlock) {
            //     unlockTimer -= Time.deltaTime;
            //     if (unlockTimer <= 0) {
            //         athleteState = AthleteState.Normal;
            //     }
            // }
        }

        private void OnBump() {
            // Bump(true);
        }

        private void OnBumpAcross() {
            // Bump(false);
        }

        private void OnTargetMoved() {
            // go to NormalState
        }

        private void OnBallHitGround() {
            // go to PostPointState
        }
    }
}
