using UnityEngine;
using KotB.StatePattern;
using KotB.StatePattern.AIStates;
using KotB.Actors;

namespace KotB.Testing
{
    public class AIDefenseProfile : MonoBehaviour
    {
        private EventPredicate ballHitGroundPredicate;

        private void Awake() {
            AI ai = GetComponent<AI>();

            var defenseState = new DefenseState(ai);

            ballHitGroundPredicate = new EventPredicate(ai.StateMachine);

            TransitionProfile defenseProfile = new TransitionProfile("Defense");

            defenseProfile.AddAnyTransition(defenseState, ballHitGroundPredicate);
            defenseProfile.SetStartingState(defenseState);

            ai.StateMachine.AddProfile(defenseProfile, true);
        }

        private void OnDisable() {
            ballHitGroundPredicate.Cleanup();
        }

        public void OnBallHitGround() {
            ballHitGroundPredicate.Trigger();            
        }
    }
}
