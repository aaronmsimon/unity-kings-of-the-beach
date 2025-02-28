using System;
using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;

namespace KotB.Stats
{
    public enum StatTypes {
        None,
        Serve,
        ServiceAce,
        ServiceError,
        Attack,
        AttackKill,
        AttackError,
        Block,
        Dig,
        Set,
    }

    public class StatKeeper : MonoBehaviour
    {
        private Dictionary<StatTypes, Action<Athlete>> statHandlers;

        private void Awake() {
            InitializeStatHandlers();
        }

        private void InitializeStatHandlers() {
            statHandlers = new Dictionary<StatTypes, Action<Athlete>> {
                { StatTypes.Serve, (athlete) => athlete.AthleteStats.ServeAttempts++ },
                { StatTypes.ServiceAce, (athlete) => athlete.AthleteStats.ServeAces++ },
                { StatTypes.ServiceError, (athlete) => athlete.AthleteStats.ServeErrors++ }
            };
        }

        public void OnStatEvent(Athlete athlete, StatTypes statType) {
            if (statHandlers.TryGetValue(statType, out var handler)) {
                handler(athlete);
            } else {
                Debug.LogWarning($"No handler found for stat type: {statType}");
            }
        }
    }
}
