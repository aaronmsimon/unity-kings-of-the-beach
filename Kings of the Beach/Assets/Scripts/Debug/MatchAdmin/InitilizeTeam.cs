using System.Collections.Generic;
using UnityEngine;
using KotB.Match;

namespace KotB.Testing
{
    public class InitializeTeam : MonoBehaviour
    {
        [SerializeField] private List<TeamSO> teams;

        private void Awake() {
            foreach (TeamSO team in teams) {
                team.Initialize();
                Debug.Log($"Team {team.name} initialized.");
            }
        }
    }
}
