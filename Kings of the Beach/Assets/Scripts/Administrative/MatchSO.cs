using UnityEngine;

[CreateAssetMenu(fileName = "Match", menuName = "Game/Match Data")]
public class MatchSO : ScriptableObject
{
    [Header("Service")]
    [SerializeField] private SkillsSO servicePlayer;

    public SkillsSO ServicePlayer {
        get {
            return servicePlayer;
        }
        set {
            servicePlayer = value;
        }
    }
}
