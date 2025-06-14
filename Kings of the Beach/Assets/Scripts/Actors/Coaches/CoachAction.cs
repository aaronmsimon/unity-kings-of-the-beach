using UnityEngine;

namespace KotB.Actors
{
    [RequireComponent(typeof(Coach))]
    public abstract class CoachAction : MonoBehaviour
    {
        protected Coach coach;

        private void Start() {
            coach = GetComponent<Coach>();
        }

        public abstract void Execute();
    }
}
