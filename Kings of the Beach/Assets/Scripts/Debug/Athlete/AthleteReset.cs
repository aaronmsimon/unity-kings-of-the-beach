using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class AthleteReset : MonoBehaviour
    {
        [SerializeField] private Coach coach;

        private Athlete athlete;
        private Vector3 startPos;

        private void Awake() {
            athlete = GetComponent<Athlete>();
            startPos = athlete.transform.position;
        }

        private void OnEnable() {
            coach.BallInfo.BallGiven += ResetPosition;
        }

        private void OnDisable() {
            coach.BallInfo.BallGiven -= ResetPosition;
        }

        private void ResetPosition() {
            athlete.transform.position = startPos;
            athlete.FaceOpponent();
        }
    }
}
