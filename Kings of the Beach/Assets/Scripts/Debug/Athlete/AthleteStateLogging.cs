using UnityEngine;
using KotB.Actors;
using KotB.StatePattern;

namespace KotB.Testing
{
    public class AthleteStateLogging : MonoBehaviour
    {
        private Athlete athlete;

        private void Awake()
        {
            athlete = GetComponent<Athlete>();
        }

        private void Start()
        {
            athlete.StateMachine.StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            athlete.StateMachine.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(IState state)
        {
            Debug.Log($"{athlete.Skills.AthleteName} state has changed from {athlete.StateMachine.CurrentState} to {state}");
        }
    }
}
