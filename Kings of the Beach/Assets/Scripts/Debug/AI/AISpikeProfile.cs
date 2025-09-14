using UnityEngine;
using KotB.Actors;
using KotB.StatePattern;
using KotB.StatePattern.AIStates;

namespace KotB.Testing
{
    public class AISpikeProfile : MonoBehaviour
    {
        private AI ai;
        private TransitionProfile spikeProfile;
        private EventPredicate ballHitGroundPredicate;
        private EventPredicate targetSetPredicate;

        private Vector3 defaultPos;

        private void Awake() {
            ai = GetComponent<AI>();
        }

        private void Start() {
            defaultPos = ai.transform.position;

            SetupSpikeProfile();
            Reset();
        }

        private void OnEnable() {
            ai.ReachedTargetPos += ai.FaceOpponent;
        }

        private void OnDisable() {
            ballHitGroundPredicate.Cleanup();
            targetSetPredicate.Cleanup();

            ai.ReachedTargetPos -= ai.FaceOpponent;
        }

        public void OnBallHitGround() {
            ballHitGroundPredicate.Trigger();
            Reset();
        }

        private void Reset() {
            ai.TargetPos = defaultPos;
            ai.BallInfo.HitsForTeam = 1;
        }

        private void SetupSpikeProfile() {
            var defenseState = new DefenseState(ai);
            var digReadyState = new DigReadyState(ai);

            ballHitGroundPredicate = new EventPredicate(ai.StateMachine);
            targetSetPredicate = new EventPredicate(ai.StateMachine);

            ballHitGroundPredicate = new EventPredicate(ai.StateMachine);
            ai.BallInfo.TargetSet += targetSetPredicate.Trigger;

            spikeProfile = new TransitionProfile("Spike");

            spikeProfile.AddAnyTransition(defenseState, ballHitGroundPredicate);
            spikeProfile.AddTransition(defenseState, digReadyState, new AndPredicate(
                targetSetPredicate,
                new FuncPredicate(() => Mathf.Sign(ai.BallInfo.TargetPos.x) == ai.CourtSide),
                new FuncPredicate(() => ai.BallInfo.Possession == ai.CourtSide)
            ));
            spikeProfile.AddTransition(digReadyState, defenseState, ai.DigToDefensePredicate);
            spikeProfile.SetStartingState(defenseState);

            ai.StateMachine.AddProfile(spikeProfile, true);
        }
    }
}
