using UnityEngine;
using KotB.Actors;
using RoboRyanTron.Unite2017.Variables;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Match
{
    [System.Serializable]
    public class Team
    {
        [SerializeField] private StringVariable teamName;
        [SerializeField] private AthleteInfo[] athletesInfo = new AthleteInfo[2];
        [SerializeField] private FloatVariable score;
        [SerializeField] private int courtSide;
        [SerializeField] private GameEvent scoreChanged;

        public Athlete[] Athletes { get; private set; }
        public bool Serving { get; set; }
        public Athlete Server { get; set; }
        private int maxAthletes = 2;

        public Team(AthleteInfo athleteInfo1, AthleteInfo athleteInfo2) {
            athletesInfo[0] = athleteInfo1;
            athletesInfo[1] = athleteInfo2;

            SetScore(0);
        }

        public void AssignAthlete(Athlete athlete) {
            if (Athletes.Length < maxAthletes) {
                Athletes[Athletes.Length] = athlete;
                if (Athletes.Length == maxAthletes) SetServer(athlete);
            }
        }

        public void SetScore(int amount) {
            score.Value = amount;
            scoreChanged.Raise();
        }

        public void AddScore(int amount) {
            score.Value += amount;
            scoreChanged.Raise();
        }

        public void SetServer(Athlete athlete) {
            Server = athlete;
        }

        public void SwitchServer() {
            Server = Server == Athletes[0] ? Athletes[1] : Athletes[0];
        }

        public StringVariable TeamName { get { return teamName; } }
        public AthleteInfo[] AthletesInfo { get { return athletesInfo; } }
        public FloatVariable Score { get { return score; } }
        public int CourtSide { get { return courtSide; } }
    }
}
