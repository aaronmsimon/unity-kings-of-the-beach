using UnityEngine;

namespace KotB.Match
{
    [System.Serializable]
    public class AthleteInfo
    {
        [SerializeField] private SkillsSO skills;
        [SerializeField] private bool computerControlled;
        [SerializeField] private Outfit outfit;
        [SerializeField] private Material top;
        [SerializeField] private Material bottom;

        // public AthleteInfo(SkillsSO skills, bool computerControlled, Outfit outfit, Material top, Material bottom) {
        //     this.skills = skills;
        //     this.computerControlled = computerControlled;
        //     this.outfit = outfit;
        //     this.top = top;
        //     this.bottom = bottom;
        // }

        public SkillsSO Skills { get { return skills; } }
        public bool ComputerControlled { get { return computerControlled; } }
        public Outfit Outfit { get { return outfit; } }
        public Material Top { get { return top; } }
        public Material Bottom { get { return bottom; } }
    }
}
