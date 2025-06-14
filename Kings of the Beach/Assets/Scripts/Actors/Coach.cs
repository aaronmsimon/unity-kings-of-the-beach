using System.Collections;
using UnityEngine;

namespace KotB.Actors
{
    public class Coach : Athlete
    {
        private enum CoachType { BumpTarget }

        [Header("Coach Configuration")]
        [SerializeField] private CoachType coachType;

        private CoachAction[] coachActions;

        private bool hasBall = false;

        protected override void Start() {
            base.Start();

            FaceOpponent();
            Reset();

            coachActions = GetComponents<CoachAction>();
        }

        public void OnBallHitGround() {
            Reset();
        }

        protected void Reset() {
            StartCoroutine(TakeBall());
        }

        private IEnumerator TakeBall() {
            yield return new WaitForSeconds(1);

            ballInfo.GiveBall(this);
            hasBall = true;
            FaceOpponent();
        }

        public void CoachAction() {
            if (hasBall) {
                coachActions[0].Execute();
                hasBall = false;
            }
        }
    }
}
