using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class CoachActionOnInput : MonoBehaviour
    {
        [SerializeField] private Coach coach;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                coach.CoachAction();
            }
        }
    }
}
