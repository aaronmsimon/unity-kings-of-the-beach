using UnityEngine;
using KotB.Match;
using KotB.Actors;

namespace KotB.Testing
{
    public class AddToTeam : MonoBehaviour
    {
        [SerializeField] private TeamSO team;

        private Athlete athlete;

        private void Awake() {
            athlete = GetComponent<Athlete>();
        }

        private void Start() {
            foreach (TeamSO team in athlete.MatchInfo.Teams) {
                if (team == this.team) {
                    team.AddAthlete(athlete);
                    Debug.Log($"Athlete {athlete} has been added to {team.name}. There are now {team.Athletes.Count} athlete(s) on the team.");
                }
            }
        }
    }
}
