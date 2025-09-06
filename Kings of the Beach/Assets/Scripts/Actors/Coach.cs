using System.Collections;
using System;
using UnityEngine;

namespace KotB.Actors
{
    public class Coach : Athlete
    {
        public event Action CoachActionPerformed;

        private CoachAction coachAction;

        private bool hasBall = false;

        protected override void Start() {
            base.Start();

            FaceOpponent();
            Reset();

            int coachActionCount = GetComponents<CoachAction>().Length;
            if (coachActionCount != 1) {
                Debug.Log($"You must have just one CoachAction on a Coach; {coachActionCount} found.");
                return;
            }

            coachAction = GetComponent<CoachAction>();
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
                coachAction.Execute();
                CoachActionPerformed?.Invoke();
                hasBall = false;
            }
        }
    }
}
