using System.Collections;
using UnityEngine;

namespace KotB.Actors
{
    public class Coach : Athlete
    {
        private enum CoachType { BumpTarget }

        [Header("Coach Configuration")]
        [SerializeField] private CoachType coachType;

        private CoachAction coachAction;

        private bool hasBall = false;

        protected override void Start() {
            base.Start();

            FaceOpponent();
            Reset();
        }

        protected override void Update() {
            switch (coachType) {
                case CoachType.BumpTarget:
                    coachAction = new BumpTargetCoach(new Vector2(-2,0));
                    break;
                default:
                    break;
            }
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
                coachAction.Execute(this);
                hasBall = false;
            }
        }
    }
}
