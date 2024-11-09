using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "Team Config", menuName = "Game/Config/Team")]
    public class TeamConfigSO : ScriptableObject
    {
        [SerializeField] private StringVariable teamName;
        [SerializeField] private FloatVariable score;

        public StringVariable TeamName { get { return teamName; } }
        public FloatVariable Score { get { return score; } }
        public List<AthleteConfig> AthleteConfigs { get; set; }
    }
}
