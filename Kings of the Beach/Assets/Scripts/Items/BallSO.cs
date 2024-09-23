using KotB.Actors;
using UnityEngine;

[CreateAssetMenu(fileName = "BallInfo", menuName = "Game/Ball Info")]
public class BallSO : ScriptableObject
{
    public Vector3 Position { get; set; }
    public Vector3 Target { get; set; }
    public BallState ballState { get; set; }
    public int Possession { get; set; }
    public int HitsForTeam { get; set; }
    public Athlete lastPlayerToHit { get; set; }

    public Athlete ballHeldBy { get; private set; }

    public void GiveBall(Athlete athlete) {
        ballHeldBy = athlete;
        ballState = BallState.Held;
    }

    public void NoOneHoldingBall() {
        ballHeldBy = null;
    }
}
