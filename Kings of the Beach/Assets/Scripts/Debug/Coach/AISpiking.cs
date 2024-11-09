using UnityEngine;
using KotB.Actors;
using KotB.Match;

namespace KotB.Testing
{
    public class AISpiking : MonoBehaviour
    {
        [SerializeField] private AI ai;
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private MatchInfoSO matchInfo;
        [SerializeField] private AI opponent1;
        [SerializeField] private AI opponent2;

        private Coach coach;
        private Vector3 aiStartPos;

        private void Awake() {
            coach = GetComponent<Coach>();
        }

        private void Start() {
            if (matchManager != null) {
                matchManager.StateMachine.ChangeState(matchManager.InPlayState);
                // ai = (AI)matchManager.MatchInfo.Teams[0].athletes[0];
                aiStartPos = new Vector3(-3, 0.01f, -2);
            } else {
                // matchInfo.Teams[0].athletes.Clear();
                // matchInfo.Teams[0].athletes.Add(ai);
                // matchInfo.Teams[1].athletes.Clear();
                // matchInfo.Teams[1].athletes.Add(opponent1);
                // matchInfo.Teams[1].athletes.Add(opponent2);
                aiStartPos = ai.transform.position;
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.B)) {
                SpikeReady();
            }
        }

        private void SpikeReady() {
            ai.transform.position = aiStartPos;
            ai.StateMachine.ChangeState(ai.DefenseState);
            ai.transform.forward = Vector3.right * -ai.CourtSide;
            ai.BallInfo.HitsForTeam = 1;

            coach.TakeBall();
        }
    }
}
