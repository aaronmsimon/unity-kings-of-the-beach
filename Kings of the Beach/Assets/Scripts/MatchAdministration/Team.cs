using System.Collections.Generic;
using KotB.Actors;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Match
{
    public class Team
    {
        public StringVariable TeamName { get; set; }
        public List<Athlete> Athletes { get; private set; }
        public FloatVariable Score { private get; set; }
        public int CourtSide { get; private set; }
        public bool Serving { get; set; }
        public Athlete Server { get; set; }

        public Team(StringVariable teamName, int startingCourtSide) {
            TeamName = teamName;
            CourtSide = startingCourtSide;
            SetScore(0);
        }

        public void AddAthlete(Athlete athlete) {
            athlete.CourtSide = CourtSide;
            Athletes.Add(athlete);
        }

        public void SetScore(int value) {
            Score.Value = value;
        }

        public void AddScore(int amount) {
            Score.Value += amount;
        }

        public void SwitchSides() {
            CourtSide = -CourtSide;
            foreach (Athlete athlete in Athletes) {
                athlete.CourtSide = CourtSide;
            }
        }
    }
}
