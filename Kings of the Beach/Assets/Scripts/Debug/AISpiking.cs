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

        private void Start() {
            SpikeReady();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SpikeReady();
            }
        }

        private void SpikeReady() {
            ai.transform.position = aiStartPos;
            ai.StateMachine.ChangeState(ai.DefenseState);
            ai.transform.forward = Vector3.right;
            ai.BallInfo.HitsForTeam = 1;

            coach.BallInfo.GiveBall(coach);
        }
    }
}
