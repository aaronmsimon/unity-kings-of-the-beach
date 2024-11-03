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

        public Athlete[] Athletes { get; private set; }
        public bool Serving { get; set; }
        public Athlete Server { get; private set; }

        private int maxAthletes = 2;

        public void AssignAthlete(Athlete athlete) {
            if (Athletes.Length < maxAthletes) {
                Athletes[Athletes.Length] = athlete;
                if (Athletes.Length == maxAthletes) SetServer(athlete);
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
            Server = Server == Athletes[0] ? Athletes[1] : Athletes[0];
        }

        public TeamSO TeamInfo { get { return teamInfo; } }
    }
}
