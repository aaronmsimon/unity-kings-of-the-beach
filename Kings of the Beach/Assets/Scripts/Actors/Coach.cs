using System.Collections;
using UnityEngine;

namespace KotB.Actors
{
    public abstract class Coach : Athlete
    {
        [Header("Target Area")]
        [SerializeField] protected Vector2 targetZonePos;
        [SerializeField] protected Vector2 targetZoneSize;
        [SerializeField] protected bool showTargetZone;
        [SerializeField] protected Color targetZoneColor = Color.red;

        private bool hasBall = false;

        protected override void Start() {
            base.Start();

            Reset();
        }

        public void OnBallHitGround() {
            Reset();
        }

        protected virtual void Reset() {
            StartCoroutine(TakeBall());
        }

        private IEnumerator TakeBall() {
            yield return new WaitForSeconds(1);

            ballInfo.GiveBall(this);
            hasBall = true;
            FaceOpponent();
            // BallInfo.HitsForTeam = resetHitCounterAmount;
            // animator.Play("HoldBall");
            // resetBallEvent.Raise();
        }

        public void CoachAction() {
            if (hasBall) {
                PerformCoachAction();
                hasBall = false;
            }
        }

        protected abstract void PerformCoachAction();
        
        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();

            Helpers.DrawTargetZone(targetZonePos, targetZoneSize, targetZoneColor, showTargetZone);
        }
    }
}
