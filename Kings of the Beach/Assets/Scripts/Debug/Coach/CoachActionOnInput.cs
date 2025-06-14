using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class CoachActionOnInput : MonoBehaviour
    {
        [Header("User Input")]
        [SerializeField] private InputReader inputReader;

        private Coach coach;

        private void Start() {
            coach = GetComponent<Coach>();
        }

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
