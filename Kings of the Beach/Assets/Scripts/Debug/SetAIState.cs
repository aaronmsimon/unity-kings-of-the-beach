using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.AIStates;

namespace KotB.Testing
{
    public class SetAIState : MonoBehaviour
    {
        private AI ai;
        
        private void Start() {
            ai = GetComponent<AI>();
            DefenseState defenseState = new DefenseState(ai);
            ai.StateMachine.ChangeState(defenseState);
        }
    }
}
