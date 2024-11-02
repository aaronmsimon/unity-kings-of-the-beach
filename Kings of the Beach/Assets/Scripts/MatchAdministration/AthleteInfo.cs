using UnityEngine;

[System.Serializable]
public class AthleteInfo
{
    [SerializeField] private SkillsSO skills;
    [SerializeField] private bool computerControlled;
    [SerializeField] private Outfit outfit;
    [SerializeField] private Material top;
    [SerializeField] private Material bottom;

    public SkillsSO Skills { get { return skills; } }
    public bool ComputerControlled { get { return computerControlled; } }
    public Outfit Outfit { get { return outfit; } }
    public Material Top { get { return top; } }
    public Material Bottom { get { return bottom; } }
}
