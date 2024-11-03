using UnityEngine;
using KotB.Actors;
using KotB.Match;

namespace KotB.Testing
{
    public class AISpiking : MonoBehaviour
    {
        [SerializeField] private AI ai;
        [SerializeField] private MatchManager matchManager;

        private Coach coach;
        private Vector3 aiStartPos;

        private void Awake() {
            coach = GetComponent<Coach>();
        }

        private void Start() {
            matchManager.StateMachine.ChangeState(matchManager.InPlayState);
            ai = (AI)matchManager.MatchInfo.Teams[0].athletes[0];
            aiStartPos = new Vector3(-3, 0.01f, -2);
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
