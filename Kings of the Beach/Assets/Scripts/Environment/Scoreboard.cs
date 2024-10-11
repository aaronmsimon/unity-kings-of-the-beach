using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Environment
{
    public class Scoreboard : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        [SerializeField] private FloatVariable team1Score;
        [SerializeField] private FloatVariable team2Score;

        [Header("Game Objects")]
        [SerializeField] private TextMeshPro team1ScoreText;
        [SerializeField] private TextMeshPro team2ScoreText;

        private void Start() {
            team1ScoreText.text = "0";
            team2ScoreText.text = "0";
        }

        public void OnScoreUpdate() {
            team1ScoreText.text = team1Score.Value.ToString();
            team2ScoreText.text = team2Score.Value.ToString();
        }
    }
}
