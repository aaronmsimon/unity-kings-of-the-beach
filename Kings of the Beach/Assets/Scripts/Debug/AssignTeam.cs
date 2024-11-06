using UnityEngine;
using KotB.Actors;
using KotB.Match;
using KotB.StatePattern.AIStates;
using KotB.StatePattern.PlayerStates;

namespace KotB.Testing
{
    public class AssignTeams : MonoBehaviour
    {
        [SerializeField] private Athlete athlete1;
        [SerializeField] private Athlete athlete2;
        [SerializeField][Range(0,1)] private int teamIndex;
        [SerializeField] private MatchInfoSO matchInfo;

        private Coach coach;

        private void Awake() {
            coach = GetComponent<Coach>();
        }

        private void Start() {
            matchInfo.Teams[teamIndex].athletes.Clear();
            matchInfo.Teams[teamIndex].AssignAthlete(athlete1);
            matchInfo.Teams[teamIndex].AssignAthlete(athlete2);

            OnBallTaken();
        }

        private void OnEnable() {
            coach.BallTaken += OnBallTaken;
        }

        private void OnDisable() {
            coach.BallTaken -= OnBallTaken;
        }

        private void OnBallTaken() {
            StartState(athlete1);
            StartState(athlete2);
        }

        private void StartState(Athlete athlete) {
            if (athlete is AI ai) {
                DefenseState defenseState = new DefenseState(ai);
                ai.StateMachine.ChangeState(defenseState);
            } else {
                NormalState normalState = new NormalState((Player)athlete);
                athlete.StateMachine.ChangeState(normalState);
            }
        }
    }   
}
