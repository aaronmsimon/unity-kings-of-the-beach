using UnityEngine;
using KotB.Items;
using KotB.Match;
using KotB.Actors;

namespace KotB.Testing
{
    public class AIBlocker : MonoBehaviour
    {
        [SerializeField] private BallSO ballInfo;
        [SerializeField] private MatchInfoSO matchInfo;
        [SerializeField] private Coach coach;
        [SerializeField] private Player player;

        private void Start() {
            SetupTeam();
            SetBallSOHitsForTeam(1);
        }

        public void OnBallHitGround() {
            SetBallSOHitsForTeam(1);
        }

        private void SetBallSOHitsForTeam(int hits) {
            ballInfo.HitsForTeam = hits;
        }

        private void SetupTeam() {
            TeamSO team =  matchInfo.Teams[0];
            team.Initialize(-1);
            team.AddAthlete(coach);
            team.AddAthlete(player);
        }
    }
}
