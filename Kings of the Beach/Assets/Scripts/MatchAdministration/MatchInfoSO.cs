using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KotB.StatePattern;
using KotB.Actors;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "MatchInfo", menuName = "Game/Match Info")]
    public class MatchInfoSO : ScriptableObject
    {
        [SerializeField] private List<TeamSO> teams;
        [SerializeField] private FloatVariable scoreToWin;

        private IState currentState;
        private int teamServeIndex;

        public event Action TransitionToPrePointState;
        public event Action TransitionToServeState;
        public event Action MatchInitialized;
        public event Action<bool> TogglePause;

        public void Initialize() {
            teamServeIndex = 0;
            MatchInitialized?.Invoke();
        }

        public void TransitionToPrePointStateEvent() {
            TransitionToPrePointState?.Invoke();
        }
        
        public void TransitionToServeStateEvent() {
            TransitionToServeState?.Invoke();
        }

        public void TogglePauseEvent(bool paused) {
            TogglePause?.Invoke(paused);
        }

        public Athlete GetTeammate(Athlete athlete) {
            foreach (TeamSO team in teams)
            {
                if (team.Athletes.Contains(athlete))
                {
                    return team.Athletes.FirstOrDefault(a => a != athlete);
                }
            }
            return null;
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
            return teams[teamServeIndex].GetServer();
        }

        public void SideOut() {
            teams[teamServeIndex].SwitchServer();
            teamServeIndex++;
            if (teamServeIndex > teams.Count - 1) teamServeIndex = 0;
        }

        public void TeamsSwitchSides() {
            foreach (TeamSO team in teams) {
                team.SwitchSides();
            }
        }

        //---- PROPERTIES ----
        public IState CurrentState { get { return currentState; } set { currentState = value; } }
        public List<TeamSO> Teams { get { return teams; } }
        public int TeamServeIndex => teamServeIndex;
        public float ScoreToWin => scoreToWin.Value;
    }
}
