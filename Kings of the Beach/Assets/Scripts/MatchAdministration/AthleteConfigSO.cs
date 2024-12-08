using UnityEngine;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "AthleteConfig", menuName = "Game/Athlete Config")]
    public class AthleteConfigSO : ScriptableObject
    {
        public SkillsSO skills;
        public bool computerControlled;
        public Outfit outfit;
        public Material top;
        public Material bottom;
    }
}
