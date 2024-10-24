using UnityEngine;

public enum PositionType {
    Blocker,
    Defender
}

[CreateAssetMenu(fileName = "Skills", menuName = "Game/Actor")]
public class SkillsSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float targetLockDistance;
    [SerializeField] private float reactionTime;

    [Header("Accuracy")]
    [SerializeField][Range(1,10)] private float passAccuracy;
    [SerializeField][Range(1,10)] private float inBoundsJudgement;

    [Header("Range")]
    [SerializeField] private float digRange;

    [Header("Serving")]
    [SerializeField][Range(1,10)] private int serving;
    [SerializeField][Range(1,10)] private int servePower;

    [Header("Behavior")]
    [SerializeField] private float height = 1.8f;
    [SerializeField] private float jumpHeight = 2.1f;
    [SerializeField] private float reachPct = 0.3f;
    [SerializeField] private Vector2 servingPartnerPos = new Vector2(5f, 1.5f);
    [SerializeField] private Vector2 defensePos = new Vector2(5, 2);
    [SerializeField] private float offenseXPos = 1.25f;

    [Header("Position")]
    [SerializeField] private PositionType playerPosition;

    private Vector3 position;

    public float MoveSpeed { get { return moveSpeed; } }
    public float TargetLockDistance { get { return targetLockDistance; } }
    public float ReactionTime { get { return reactionTime; } }
    public float PassAccuracy { get { return passAccuracy; } }
    public float InBoundsJudgement { get { return inBoundsJudgement; } }
    public PositionType PlayerPosition { get { return playerPosition; } }
    public float DigRange { get { return digRange; } }
    public float Height { get { return height; } }
    public float JumpHeight { get { return jumpHeight; } }
    public float ReachPct { get { return reachPct; } }
    public int Serving { get { return serving; } }
    public int ServePower { get { return servePower; } }

    public Vector3 ServingPartnerPos { get { return new Vector3(servingPartnerPos.x, 0.01f, servingPartnerPos.y); } }
    public Vector2 DefensePos { get { return defensePos; } }
    public float OffenseXPos { get { return offenseXPos; } }

    public Vector3 Position {
        get { return position; }
        set { position = value; }
    }
}
