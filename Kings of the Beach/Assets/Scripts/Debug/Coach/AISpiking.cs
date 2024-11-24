using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class AISpiking : MonoBehaviour
    {
        [SerializeField] private AI ai;

        private Coach coach;
        private Vector3 aiStartPos;

        private void Awake() {
            coach = GetComponent<Coach>();
            aiStartPos = ai.transform.position;
        }

        private void OnEnable() {
            coach.BallTaken += SpikeReady;
        }

        private void OnDisable() {
            coach.BallTaken -= SpikeReady;
        }

        private void SpikeReady() {
            ai.transform.position = aiStartPos;
            ai.StateMachine.ChangeState(ai.DefenseState);
            ai.transform.forward = Vector3.right * -ai.CourtSide;
            ai.BallInfo.HitsForTeam = 1;
        }
    }
}
