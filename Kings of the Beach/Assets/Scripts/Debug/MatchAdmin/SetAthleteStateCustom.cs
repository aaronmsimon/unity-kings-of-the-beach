using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.AIStates;

namespace KotB.Testing
{
    public class SetAthleteStateCustom : MonoBehaviour
    {
        private DefenseState defenseState;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.J)) {
                AI ai = FindObjectOfType<AI>();
                defenseState = new DefenseState(ai);
                // ai.StateMachine.ChangeState(defenseState);
            }
        }
    }
}
