using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "Team", menuName = "Game/Team")]
    public class TeamSO : ScriptableObject
    {
        [SerializeField] private StringVariable teamName;
        [SerializeField] private List<Athlete> athletes;
        [SerializeField] private FloatVariable score;
        [SerializeField] private int courtSide;
        [SerializeField] private bool serving;
        [SerializeField] private Athlete server;

        public List<AthleteConfig> AthleteConfigs { get; set; }

        public void Initialize() {
            athletes = new List<Athlete>();
            SetScore(0);
        }

        public void AddAthlete(Athlete athlete) {
            athlete.CourtSide = courtSide;
            athletes.Add(athlete);
        }

        public void SetScore(int value) {
            score.Value = value;
        }

        public void AddScore(int amount) {
            score.Value += amount;
        }

        public void SwitchSides() {
            courtSide = -courtSide;
            foreach (Athlete athlete in athletes) {
                athlete.CourtSide = courtSide;
            }
        }

        public void SideOut() {
            serving = !serving;

            int serverIndex = -1;
            for (int i = 0; i < athletes.Count; i++) {
                if (server = athletes[i]) {
                    serverIndex = i;
                }
            }
            if (serverIndex < athletes.Count - 1) {
                serverIndex++;
            } else {
                serverIndex = 0;
            }
            server = athletes[serverIndex];
        }

        public StringVariable TeamName { get { return teamName; } set { teamName = value; } }
        public List<Athlete> Athletes => athletes;
        public int CourtSide { get { return courtSide; } set { courtSide = value; } }
        public bool Serving { get { return serving; } set { serving = value; } }
        public Athlete Server { get { return server; } set { server = value; } }
    }
}
