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
        [SerializeField] private Athlete[] athletes = new Athlete[2];
        [SerializeField] private FloatVariable score;
        [SerializeField] private int courtSide;
        [SerializeField] private GameEvent scoreChanged;

        public Athlete[] Athletes {
            get {
                return athletes;
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

        public int CourtSide { get { return courtSide; } }
        public StringVariable TeamName { get { return teamName; } }
        public FloatVariable Score { get { return score; } }
    }
}
