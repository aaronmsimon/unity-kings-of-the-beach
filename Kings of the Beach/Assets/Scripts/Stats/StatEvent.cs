using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;

namespace KotB.Stats
{
    [CreateAssetMenu]
    public class StatEvent : ScriptableObject
    {
        private readonly List<StatEventListener> eventListeners = 
            new List<StatEventListener>();

        public void Raise(Athlete athlete, StatTypes statType)
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised(athlete, statType);
        }

        public void RegisterListener(StatEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(StatEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}
