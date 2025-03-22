using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;
using KotB.Items;

namespace KotB.Testing
{
    public class AIBlockTesting : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField][Range(0,2)] private int hitsForTeam;

        [Header("Athletes")]
        [SerializeField] private List<Athlete> athletes = new List<Athlete>();
        [SerializeField] private Coach coach;

        [Header("Scriptable Objects")]
        [SerializeField] private BallSO ballInfo;

        private Player player;
        private AI ai;
        private SphereCollider aiCollider;
        private bool ballSpiked;

        private void Awake() {
            foreach (Athlete athlete in athletes) {
                if (athlete.TryGetComponent<AI>(out AI ai)) {
                    this.ai = ai;
                    aiCollider = ai.GetComponent<SphereCollider>();
                }
                if (athlete.TryGetComponent<Player>(out Player player)) {
                    this.player = player;
                }
            }            
        }

        private void Start() {
            ResetStates();
            ballInfo.TimeSinceLastHit = -100;
        }

        private void OnEnable() {
            coach.BallTaken += OnBallTaken;
            ballInfo.BallSpiked += OnBallSpiked;
        }

        private void OnDisable() {
            coach.BallTaken -= OnBallTaken;
            ballInfo.BallSpiked -= OnBallSpiked;
        }

        private void Update() {
            if (ballSpiked) {
                float dist = DistanceToSphere(ballInfo.Position, aiCollider.center, aiCollider.radius);
                Debug.Log($"At ball.x = {ballInfo.Position.x:F2}, distance to collider = {dist:F3} (collider active: {aiCollider.enabled}) [{ai.StateMachine.CurrentState}] [{ai.BallInfo.Possession}]");
            }
        }

        private void ResetStates() {
            foreach (Athlete athlete in athletes) {
                if (athlete.TryGetComponent<SetAthleteState>(out SetAthleteState setAthleteState)) {
                    setAthleteState.SetState();
                }
            }
            ballInfo.HitsForTeam = hitsForTeam;
        }

        private void OnBallTaken() {
            ResetStates();
        }

        private void OnBallSpiked() {
            ballSpiked = true;
        }

        public void OnBallHitGround() {
            ballSpiked = false;
        }

        private float DistanceToSphere(Vector3 point, Vector3 sphereCenter, float sphereRadius)
        {
            // Calculate the distance between the point and sphere center
            float distance = Vector3.Distance(point, sphereCenter);
            
            // If the point is inside the sphere, return 0 (or a negative value if you prefer)
            if (distance <= sphereRadius)
                return 0f; // Point is inside or on the sphere
            
            // Otherwise, return the distance to the sphere surface
            return distance - sphereRadius;
        }
    }
}
