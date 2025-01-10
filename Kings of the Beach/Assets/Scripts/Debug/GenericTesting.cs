using UnityEngine;
using KotB.Match;
using KotB.Actors;

namespace KotB.Testing
{
    public class GenericTesting : MonoBehaviour
    {
        [SerializeField] private MatchInfoSO matchInfo;
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.J)) {
                Athlete[] athletes = FindObjectsOfType<Athlete>();
                foreach (Athlete athlete in athletes) {
                    TeamSO team = matchInfo.GetTeam(athlete);
                    Debug.Log($"Team {team.TeamName.Value} has athlete {athlete.Skills.AthleteName}");
                }
            }
        }
    }
}
