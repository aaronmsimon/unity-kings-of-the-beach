using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class CoachActionOnInput : MonoBehaviour
    {
        [SerializeField] private Coach coach;

        [Header("User Input")]
        [SerializeField] private InputReader inputReader;

        private void OnEnable() {
            inputReader.testEvent += OnTestEvent;
        }

        private void OnDisable() {
            inputReader.testEvent -= OnTestEvent;
        }

        private void OnTestEvent() {
            coach.CoachAction();
        }
    }
}
