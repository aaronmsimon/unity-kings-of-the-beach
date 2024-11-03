using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KotB.StatePattern;
using KotB.Actors;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "MatchInfo", menuName = "Game/Match Info")]
    public class MatchInfoSO : ScriptableObject
    {
        [SerializeField] private Team[] teams = new Team[2];
        public int TotalPoints { get; set; }
        public int ScoreToWin { get; set; }

        private IState currentState;

        public event Action TransitionToPrePointState;
        public event Action TransitionToServeState;

        public void TransitionToPrePointStateEvent() {
            TransitionToPrePointState?.Invoke();
        }
        
        public void TransitionToServeStateEvent() {
            TransitionToServeState?.Invoke();
        }
    
        public Athlete GetTeammate(Athlete athlete)
        {
            foreach (Team team in teams)
            {
                if (team.athletes.Contains(athlete))
                {
                    return team.athletes.FirstOrDefault(a => a != athlete);
                }
            }
            return null; // Should never happen if setup correctly
        }

        public List<Athlete> GetOpponents(Athlete athlete)
        {
            foreach (Team team in teams)
            {
                if (team.athletes.Contains(athlete))
                {
                    // Return athletes from the opposing team
                    return teams.FirstOrDefault(t => t != team)?.athletes;
                }
            }
            return null;
        }

        public Team GetTeam(Athlete athlete) {
            foreach (Team team in teams)
            {
                if (team.athletes.Contains(athlete))
                {
                    return team;
                }
            }
            return null;
        }

        public Team GetOpposingTeam(Athlete athlete) {
            foreach (Team team in teams)
            {
                if (team.athletes.Contains(athlete))
                {
                    return teams.FirstOrDefault(t => t != team);
                }
            }
            return null;
        }

        public Athlete GetServer() {
            foreach (Team team in teams) {
                foreach (Athlete athlete in team.athletes) {
                    if (team.Serving && team.Server == athlete) {
                        return athlete;
                    }
                }
            }
            return null;
        }

        //---- PROPERTIES ----
        public IState CurrentState {
            get { return currentState; }
            set { currentState = value; }
        }
        public Team[] Teams { get { return teams; } set { teams = value; } }
    }
}
