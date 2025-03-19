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

        private void Start() {
            ResetStates();
            ballInfo.TimeSinceLastHit = -100;
        }

        private void OnEnable() {
            coach.BallTaken += OnBallTaken;
        }

        private void OnDisable() {
            coach.BallTaken -= OnBallTaken;
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
    }
}
