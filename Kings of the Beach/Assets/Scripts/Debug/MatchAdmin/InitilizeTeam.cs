using System.Collections.Generic;
using UnityEngine;
using KotB.Match;

namespace KotB.Testing
{
    public class InitializeTeam : MonoBehaviour
    {
        [SerializeField] private List<TeamSO> teams;

        private void Awake() {
            int teamIndex = 0;
            foreach (TeamSO team in teams) {
                team.Initialize(teamIndex == 0 ? -1 : 1);
                Debug.Log($"Team {team.name} initialized.");
                teamIndex++;
            }
        }
    }
}
