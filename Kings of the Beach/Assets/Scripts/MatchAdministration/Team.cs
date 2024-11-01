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

        public Team(Athlete athlete1, Athlete athlete2) {
            athletes[0] = athlete1;
            athletes[1] = athlete2;
            if (athlete1 is AI ai1) ai1.Teammate = athlete2;
            if (athlete2 is AI ai2) ai2.Teammate = athlete1;
            SetScore(0);
        }

        // public Team(SkillsSO player1, bool aiControlled1, SkillsSO player2, bool aiControlled2) {
        //     players[0] = player1;
        //     players[1] = player2;
        //     SetScore(0);

        //     /* or maybe:
        //         1. assign skillSO to Team (instead of Athletes)
        //         2. set if AI or player controlled (or just AI y/n)
        //         3. game mgr instantiates the players based on this list with teammate assignment as appropriate
        //      */
        // }

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
