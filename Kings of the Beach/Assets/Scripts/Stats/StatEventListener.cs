using UnityEngine;
using UnityEngine.Events;
using KotB.Actors;

namespace KotB.Stats
{
    public class StatEventListener : MonoBehaviour
    {
        [Tooltip("Stat Event to register with.")]
        [SerializeField] private StatEvent statEvent;

        [Tooltip("Response to invoke when Stat Event is raised.")]
        [SerializeField] private UnityEvent<Athlete, StatTypes> response;

        private void OnEnable()
        {
            statEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            statEvent.UnregisterListener(this);
        }

        public void OnEventRaised(Athlete athlete, StatTypes statType)
        {
            response.Invoke(athlete, statType);
        }
    }
}
