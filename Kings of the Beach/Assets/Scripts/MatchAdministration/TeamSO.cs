using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "Team", menuName = "Game/Team")]
    public class TeamSO : ScriptableObject
    {
        [SerializeField] private StringVariable teamName;
        [SerializeField] private FloatVariable score;
        // [SerializeField] private bool serving;
        // [SerializeField] private Athlete server;

        private List<AthleteConfig> athleteConfigs;

        public void Initialize() {
            athleteConfigs = new List<AthleteConfig>();
            SetScore(0);
        }

        public void AddAthleteConfig(AthleteConfig athleteConfig) {
            athleteConfigs.Add(athleteConfig);
        }

        public void SetScore(int value) {
            score.Value = value;
        }

        public void AddScore(int amount) {
            score.Value += amount;
        }

        // public void SideOut() {
        //     serving = !serving;

        //     int serverIndex = -1;
        //     for (int i = 0; i < athletes.Count; i++) {
        //         if (server = athletes[i]) {
        //             serverIndex = i;
        //         }
        //     }
        //     if (serverIndex < athletes.Count - 1) {
        //         serverIndex++;
        //     } else {
        //         serverIndex = 0;
        //     }
        //     server = athletes[serverIndex];
        // }

        public StringVariable TeamName { get { return teamName; } set { teamName = value; } }
        public List<AthleteConfig> AthleteConfigs => athleteConfigs;
        // public bool Serving { get { return serving; } set { serving = value; } }
        // public Athlete Server { get { return server; } set { server = value; } }
    }
}
