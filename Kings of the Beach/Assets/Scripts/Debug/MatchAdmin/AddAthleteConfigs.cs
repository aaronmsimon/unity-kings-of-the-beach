using UnityEngine;
using KotB.Match;

namespace KotB.Testing
{
    public class AddAthleteConfigs : MonoBehaviour
    {
        [SerializeField] private TeamSO team;
        [SerializeField] private AthleteConfigSO athleteConfig;

        private void Awake() {
            team.Initialize();
            team.AddAthleteConfig(athleteConfig);
            Debug.Log($"Team {team.name} has athleteconfig with skills {athleteConfig.skills.AthleteName} added.");
        }
    }
}
