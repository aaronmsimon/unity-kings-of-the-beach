using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Match
{
    [System.Serializable]
    public class Team
    {
        [Header("Team Info")]
        [SerializeField] private TeamSO teamInfo;

        [Header("Events")]
        [SerializeField] private GameEvent scoreChanged;

        public List<Athlete> athletes = new List<Athlete>();
        public bool Serving { get; set; }
        public Athlete Server { get; private set; }

        private int maxAthletes = 2;

        public void AssignAthlete(Athlete athlete) {
            if (athletes.Count < maxAthletes) {
                athletes.Add(athlete);
                SetServer(athlete);
            }
        }

        public void SetScore(int amount) {
            teamInfo.Score.Value = amount;
            scoreChanged.Raise();
        }

        public void AddScore(int amount) {
            teamInfo.Score.Value += amount;
            scoreChanged.Raise();
        }

        public void SetServer(Athlete athlete) {
            Server = athlete;
        }

        public void SwitchServer() {
            Server = Server == athletes[0] ? athletes[1] : athletes[0];
        }

        public TeamSO TeamInfo { get { return teamInfo; } }
        public List<Athlete> Athletes { get { return athletes; } }
    }
}
