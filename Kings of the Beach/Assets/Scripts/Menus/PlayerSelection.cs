using UnityEngine;
using KotB.Match;

namespace KotB.Menus
{
    public class PlayerSelection : MonoBehaviour
    {
        [SerializeField] private AthleteConfigSO athleteConfig;
        [SerializeField] private UIGroupSelect country;
        [SerializeField] private UIGroupSelect player;
        [SerializeField] private OutfitSelection outfit;
        [SerializeField] private UIGroupSelect top;
        [SerializeField] private UIGroupSelect bottom;

        public AthleteConfigSO AthleteConfig => athleteConfig;
        public UIGroupSelect Country => country;
        public UIGroupSelect Player => player;
        public OutfitSelection Outfit => outfit;
        public UIGroupSelect Top => top;
        public UIGroupSelect Bottom => bottom;
    }
}
