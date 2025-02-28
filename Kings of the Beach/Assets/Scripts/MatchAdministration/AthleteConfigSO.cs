using UnityEngine;
using KotB.Items;
using KotB.Stats;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "AthleteConfig", menuName = "Game/Athlete Config")]
    public class AthleteConfigSO : ScriptableObject
    {
        public SkillsSO skills;
        public bool computerControlled;
        public Outfit outfit;
        public MaterialSO top;
        public MaterialSO bottom;
        public AthleteStatsSO athleteStats;
    }
}
