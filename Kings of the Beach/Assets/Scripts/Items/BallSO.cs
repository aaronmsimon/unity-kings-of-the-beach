using KotB.Actors;
using UnityEngine;

[CreateAssetMenu(fileName = "Ball", menuName = "Game/Ball")]
public class BallSO : ScriptableObject
{
    public Vector3 Position { get; set; }
    public Vector3 Target { get; set; }
    public BallState ballState { get; set; }
    public int Possession { get; set; }
    public int HitsForTeam { get; set; }
    public Athlete lastPlayerToHit { get; set; }
}
