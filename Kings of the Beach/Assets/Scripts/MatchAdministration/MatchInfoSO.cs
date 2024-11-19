using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KotB.StatePattern;
using KotB.Actors;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "MatchInfo", menuName = "Game/Match Info")]
    public class MatchInfoSO : ScriptableObject
    {
        private List<TeamSO> teams;
        private IState currentState;

        public event Action TransitionToPrePointState;
        public event Action TransitionToServeState;

        public void TransitionToPrePointStateEvent() {
            TransitionToPrePointState?.Invoke();
        }
        
        public void TransitionToServeStateEvent() {
            TransitionToServeState?.Invoke();
        }

        public TeamSO GetTeam(Athlete athlete) {
            foreach (TeamSO team in teams)
            {
                if (team.Athletes.Contains(athlete))
                {
                    return team;
                }
            }
            return null;
        }

        public TeamSO GetOpposingTeam(Athlete athlete) {
            foreach (TeamSO team in teams)
            {
                if (team.Athletes.Contains(athlete))
                {
                    return teams.FirstOrDefault(t => t != team);
                }
            }
            return null;
        }

        public Athlete GetServer() {
            foreach (TeamSO team in teams) {
                if (team.Serving) {
                    return team.Server;
                }
            }
            return null;
        }

        //---- PROPERTIES ----
        public IState CurrentState { get { return currentState; } set { currentState = value; } }
        public List<TeamSO> Teams { get { return teams; } }
    }
}
