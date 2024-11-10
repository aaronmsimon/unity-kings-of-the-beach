using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class SetAIState : MonoBehaviour
    {
        private AI ai;

        private void Awake() {
            ai = GetComponent<AI>();
        }
        
        public void SetAIState_Defense() {
            ai.StateMachine.ChangeState(ai.DefenseState);
        }
    }
}
