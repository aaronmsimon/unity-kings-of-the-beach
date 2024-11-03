using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "Team", menuName = "Game/Team")]
    public class TeamSO : ScriptableObject
    {
        [Header("Athlete Info")]
        [SerializeField] private AthleteInfo[] athleteInfos = new AthleteInfo[2];

        [Header("Team Info")]
        [SerializeField] private StringVariable teamName;
        [SerializeField] private FloatVariable score;
        [SerializeField] private int courtSide;

        public AthleteInfo[] AthleteInfos { get { return athleteInfos; } }
        public StringVariable TeamName { get { return teamName; } }
        public FloatVariable Score { get { return score; } }
        public int CourtSide { get { return courtSide; } }
    }
}
