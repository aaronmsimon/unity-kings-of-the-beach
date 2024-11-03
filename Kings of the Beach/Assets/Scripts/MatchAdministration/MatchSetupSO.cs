using UnityEngine;

namespace KotB.Match
{
    [CreateAssetMenu(fileName = "MatchSetup", menuName = "Game/Match Setup")]
    public class MatchSetupSO : ScriptableObject
    {
        [Header("Teams")]
        [SerializeField] private Team team1;
        [SerializeField] private Team team2;
    }
}
