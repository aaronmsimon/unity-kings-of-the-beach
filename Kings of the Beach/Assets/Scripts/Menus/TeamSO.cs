using UnityEngine;

namespace KotB.Menus.Alt
{
    [CreateAssetMenu(fileName = "Team", menuName = "Menu/Team")]
    public class TeamSO : ScriptableObject
    {
        [SerializeField] private CountrySO country;
        [SerializeField] private string teamVersion;
        [SerializeField] private SkillsSO blocker;
        [SerializeField] private SkillsSO defender;
    }
}
