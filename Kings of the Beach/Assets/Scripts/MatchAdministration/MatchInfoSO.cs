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
        private List<Team> teams;
        private IState currentState;

        public event Action TransitionToPrePointState;
        public event Action TransitionToServeState;

        public void TransitionToPrePointStateEvent() {
            TransitionToPrePointState?.Invoke();
        }
        
        public void TransitionToServeStateEvent() {
            TransitionToServeState?.Invoke();
        }

        public Team GetTeam(Athlete athlete) {
            foreach (Team team in teams)
            {
                if (team.Athletes.Contains(athlete))
                {
                    return team;
                }
            }
            return null;
        }

        public Team GetOpposingTeam(Athlete athlete) {
            foreach (Team team in teams)
            {
                if (team.Athletes.Contains(athlete))
                {
                    return teams.FirstOrDefault(t => t != team);
                }
            }
            return null;
        }

        public Athlete GetServer() {
            foreach (Team team in teams) {
                foreach (Athlete athlete in team.Athletes) {
                    if (team.Serving && team.Server == athlete) {
                        return athlete;
                    }
                }
            }
            return null;
        }

        //---- PROPERTIES ----
        public IState CurrentState { get { return currentState; } set { currentState = value; } }
        public List<Team> Teams { get { return teams; } }
    }
}
