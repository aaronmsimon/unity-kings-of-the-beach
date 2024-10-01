using UnityEngine;
using KotB.Actors;

namespace KotB.Match
{
    [System.Serializable]
    public class Team
    {
        [SerializeField] private string teamName;
        [SerializeField] private Athlete[] athletes = new Athlete[2];
        [SerializeField] private int score;
        [SerializeField] private int courtSide;

        public Athlete[] Athletes {
            get {
                return athletes;
            }
        }

        public void SetScore(int amount) {
            score = amount;
        }

        public void AddScore(int amount) {
            score += amount;
        }

        public string TeamName { get { return teamName; } }
        public int Score { get { return score; } }
        public int CourtSide { get { return courtSide; } }
    }
}
