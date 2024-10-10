using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Variables;

namespace KotB.Environment
{
    public class Scoreboard : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        [SerializeField] private FloatVariable team1Score;

        [Header("Game Objects")]
        [SerializeField] private TextMeshPro team1ScoreText;

        private void Start() {
            team1ScoreText.text = "0";
        }

        public void OnScoreUpdate() {
            team1ScoreText.text = team1Score.Value.ToString();
        }
    }
}
