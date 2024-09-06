using UnityEngine;
using UnityEngine.UIElements.Experimental;

[CreateAssetMenu(fileName = "Skills", menuName = "Game/Actor")]
public class SkillsSO : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float targetLockDistance;

    public float MoveSpeed {
        get {
            return moveSpeed;
        }
    }
    public float TargetLockDistance {
        get {
            return targetLockDistance;
        }
    }
}
