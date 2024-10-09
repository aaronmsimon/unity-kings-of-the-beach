using UnityEngine;

namespace KotB.Actors
{
    [CreateAssetMenu(fileName = "Skill Values", menuName = "Game/Skill Values")]
    public class SkillValues : ScriptableObject
    {
        [Header("Serving")]
        // [SerializeField] private int serving;
        [SerializeField] private MinMax servePower;

        private MinMax skillRange = new MinMax(1, 10);

        public float ServeRate(int servePowerSkill) {
            return SkillToValueAscending(servePowerSkill, servePower);
        }

        private float SkillToValueAscending(int skill, MinMax valueRange) {
            return (skill - skillRange.min) * (valueRange.max - valueRange.min) / (skillRange.max - skillRange.min) + valueRange.min;
        }
    }
}
