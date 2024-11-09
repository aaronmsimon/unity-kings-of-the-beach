using UnityEngine;

namespace KotB.Match
{
    public class AthleteConfig
    {
        public SkillsSO Skills { get; set; }
        public bool ComputerControlled { get; set; }
        public Outfit Outfit { get; set; }
        public Material Top { get; set; }
        public Material Bottom { get; set; }
    }
}
