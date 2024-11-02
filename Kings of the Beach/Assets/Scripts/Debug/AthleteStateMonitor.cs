using UnityEngine;
using KotB.Actors;
using KotB.StatePattern;

namespace KotB.Testing
{
    public class AthleteStateMonitor : MonoBehaviour
    {
        [SerializeField] private string athleteState;

        private Athlete athlete;

        private void Start() {
            athlete = GetComponent<Athlete>();
            athlete.StateMachine.StateChanged += OnStateChanged;
        }

        private void OnDestroy() {
            athlete.StateMachine.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(IState newState) {
            athleteState = newState?.ToString();
        }
    }
}
