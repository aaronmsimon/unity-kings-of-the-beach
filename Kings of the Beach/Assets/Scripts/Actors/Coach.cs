using System.Collections;
using UnityEngine;
using KotB.StatePattern;

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
            SetupStateMachine();
        }

        protected override void Update() {
            switch (coachType) {
                case CoachType.BumpTarget:
                    coachAction = new BumpTargetCoach();
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
            // BallInfo.HitsForTeam = resetHitCounterAmount;
            // animator.Play("HoldBall");
            // resetBallEvent.Raise();
        }

        public void CoachAction() {
            if (hasBall) {
                coachAction.Execute(this);
                hasBall = false;
            }
        }

        private void SetupStateMachine() {
            // State Machine
            stateMachine = new StateMachine();
        }
    }
}
