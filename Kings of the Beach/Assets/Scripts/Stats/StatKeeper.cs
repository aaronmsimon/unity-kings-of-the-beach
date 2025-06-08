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
        BlockAttempt,
        Block,
        BlockPoint,
        BlockError,
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
                { StatTypes.ServiceError, (athlete) => athlete.AthleteStats.ServeErrors++ },
                { StatTypes.BlockAttempt, (athlete) => athlete.AthleteStats.BlockAttempts++ },
                { StatTypes.Block, (athlete) => athlete.AthleteStats.Blocks++ },
                { StatTypes.BlockPoint, (athlete) => athlete.AthleteStats.BlockPoints++ },
                { StatTypes.BlockError, (athlete) => athlete.AthleteStats.BlockErrors++ },
                { StatTypes.Attack, (athlete) => athlete.AthleteStats.Attacks++ },
                { StatTypes.AttackKill, (athlete) => athlete.AthleteStats.AttackKills++ },
                { StatTypes.AttackError, (athlete) => athlete.AthleteStats.AttackErrors++ },
                { StatTypes.Dig, (athlete) => athlete.AthleteStats.Digs++ },
            };
        }

        public void OnStatEvent(Athlete athlete, StatTypes statType) {
            if (statHandlers.TryGetValue(statType, out var handler)) {
                handler(athlete);
            } /*else {
                Debug.LogWarning($"No handler found for stat type: {statType}");
            }*/
        }
    }
}
