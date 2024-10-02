using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Variables;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TeamScore : MonoBehaviour
{
    [SerializeField] private FloatVariable score;

    private TextMeshProUGUI scoreLabel;

    private void Awake() {
        scoreLabel = GetComponent<TextMeshProUGUI>();
    }

    public void OnScoreUpdate() {
        scoreLabel.text = score.Value.ToString();
    }
}
