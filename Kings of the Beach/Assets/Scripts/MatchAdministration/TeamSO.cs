using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using KotB.Actors;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "Team", menuName = "Game/Team")]
    public class TeamSO : ScriptableObject
    {
        [SerializeField] private StringVariable teamName;
        [SerializeField] private FloatVariable score;
        [SerializeField] private FloatVariable courtSide;
        [SerializeField] private List<AthleteConfigSO> athleteConfigs;

        private List<Athlete> athletes;
        private int serverIndex;

        public void Initialize(float courtSide) {
            athletes = new List<Athlete>();
            serverIndex = 0;
            SetScore(0);
            this.courtSide.Value = courtSide;
        }

        public void AddAthleteConfig(AthleteConfigSO athleteConfig) {
            athleteConfigs.Add(athleteConfig);
        }

        public void AddAthlete(Athlete athlete) {
            athletes.Add(athlete);
        }

        public void SetScore(int value) {
            score.Value = value;
        }

        public void AddScore(int amount) {
            score.Value += amount;
        }

        public Athlete GetServer() {
            return athletes[serverIndex];
        }

        public void SwitchServer() {
            serverIndex++;
            if (serverIndex > athletes.Count - 1) serverIndex = 0;
        }

        public void SwitchSides() {
            courtSide.Value = -courtSide.Value;
        }

        public bool IsCaptain(Athlete athlete) {
            return athletes.Count > 0 && athletes[0] == athlete;
        }

        public List<AthleteConfigSO> AthleteConfigs => athleteConfigs;
        public List<Athlete> Athletes => athletes;
        public StringVariable TeamName => teamName;
        public FloatVariable CourtSide => courtSide;
    }
}
