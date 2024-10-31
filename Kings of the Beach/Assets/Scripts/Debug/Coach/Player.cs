using UnityEngine;
using KotB.Actors;

namespace KotB.Testing.Coaching
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Actors.Player player;

        private Coach coach;
        private Vector3 defaultPos;

        private void Awake() {
            coach = GetComponent<Coach>();
            defaultPos = player.transform.position;
        }

        private void OnEnable() {
            coach.BallTaken += OnBallTaken;
        }

        private void OnDisable() {
            coach.BallTaken -= OnBallTaken;
        }

        private void OnBallTaken() {
            player.transform.position = defaultPos;
            player.StateMachine.ChangeState(player.NormalState);
        }
    }
}
