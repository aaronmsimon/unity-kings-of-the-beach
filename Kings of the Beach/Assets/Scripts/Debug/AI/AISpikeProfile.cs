using UnityEngine;
using KotB.Actors;
using KotB.StatePattern;
using KotB.StatePattern.AIStates;

namespace KotB.Testing
{
    public class AISpikeProfile : MonoBehaviour
    {
        private EventPredicate ballHitGroundPredicate;
        private EventPredicate targetSetPredicate;

        private void Start() {
            AI ai = GetComponent<AI>();
            ai.TargetPos = ai.transform.position;

            var defenseState = new DefenseState(ai);
            var digReadyState = new DigReadyState(ai);

            ballHitGroundPredicate = new EventPredicate(ai.StateMachine);
            targetSetPredicate = new EventPredicate(ai.StateMachine);

            ballHitGroundPredicate = new EventPredicate(ai.StateMachine);
            ai.BallInfo.TargetSet += targetSetPredicate.Trigger;

            TransitionProfile spikeProfile = new TransitionProfile("Spike");

            spikeProfile.AddAnyTransition(defenseState, ballHitGroundPredicate);
            spikeProfile.AddTransition(defenseState, digReadyState, targetSetPredicate);
            spikeProfile.AddTransition(defenseState, digReadyState, new AndPredicate(
                targetSetPredicate,
                new FuncPredicate(() => Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide),
                new FuncPredicate(() => ai.BallInfo.Possession == ai.CourtSide)
            ));
            spikeProfile.SetStartingState(defenseState);

            ai.StateMachine.AddProfile(spikeProfile, true);
            Debug.Log("spike profile loaded");
        }

        private void OnDisable() {
            ballHitGroundPredicate.Cleanup();
            targetSetPredicate.Cleanup();
        }

        public void OnBallHitGround() {
            ballHitGroundPredicate.Trigger();
        }
    }
}
