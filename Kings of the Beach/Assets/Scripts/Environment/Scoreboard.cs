using UnityEngine;
using TMPro;
using KotB.Match;

namespace KotB.Environment
{
    public class Scoreboard : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        [SerializeField] private TeamSO team1;
        [SerializeField] private TeamSO team2;

        private TextMeshPro team1ScoreText;
        private TextMeshPro team2ScoreText;

        private void Start() {
            SetupTeamPanel("Team1", team1, out team1ScoreText);
            SetupTeamPanel("Team2", team2, out team2ScoreText);

            OnScoreUpdate();
        }

        private void SetupTeamPanel(string panelName, TeamSO team, out TextMeshPro scoreText)
        {
            Transform panel = transform.Find(panelName);

            panel.Find("Logo").GetComponent<SpriteRenderer>().sprite = team.TeamLogo;
            panel.Find("Abbr").GetComponent<TextMeshPro>().text = team.TeamAbbr.Value;
            scoreText = panel.Find("Score").GetComponent<TextMeshPro>();
        }

        public void OnScoreUpdate() {
            team1ScoreText.text = team1.Score.ToString();
            team2ScoreText.text = team2.Score.ToString();
        }
    }
}
