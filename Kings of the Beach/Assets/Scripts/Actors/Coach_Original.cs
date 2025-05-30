using System;
using UnityEngine;
using KotB.StatePattern.MatchStates;
using KotB.Match;
using RoboRyanTron.Unite2017.Events;
using KotB.Items;

namespace KotB.Actors
{
    public class Coach_Original : Athlete
    {
        private enum CoachType { Bump, Pass }

        [Header("Coach Info")]
        [SerializeField] private CoachType coachType;
        
        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private Vector2 targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor;

        [Header("Game Input")]
        [SerializeField] private InputReader inputReader;

        [Header("Game Events")]
        [SerializeField] private GameEvent resetBallEvent;
        [SerializeField][Range(0,2)] private int resetHitCounterAmount;

        public event Action BallTaken;

        private MatchManager matchManager;

        private float bumpFrames = 7;
        private float bumpAnimationTime;
        private bool canBump;

        protected override void Start() {
            base.Start();

            canBump = false;

            InPlayState inPlayState = new InPlayState(matchManager);
            matchInfo.CurrentState = inPlayState;

            TakeBall();
        }

        private void OnEnable() {
            inputReader.testEvent += OnBump;
        }
        
        private void OnDisable() {
            inputReader.testEvent -= OnBump;
        }

        protected override void Update() {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                TakeBall();
            }
            #endif

            bumpAnimationTime += Time.deltaTime;
            if (canBump && bumpAnimationTime >= bumpFrames / animationFrameRate) {
                Bump();
            }
        }

        public void TakeBall() {
            ballInfo.GiveBall(this);
            FaceOpponent();
            BallInfo.HitsForTeam = resetHitCounterAmount;
            animator.Play("HoldBall");
            BallTaken?.Invoke();
            resetBallEvent.Raise();
        }

        private void Bump() {
            float posX = UnityEngine.Random.Range(targetZonePos.x - targetZoneSize.x / 2, targetZonePos.x + targetZoneSize.x / 2);
            float posY = UnityEngine.Random.Range(targetZonePos.y - targetZoneSize.y / 2, targetZonePos.y + targetZoneSize.y / 2);

            if (coachType == CoachType.Bump) {
                Pass(new Vector3(posX, 0, posY), 7, 1.75f);
            } else {
                Player player = FindObjectOfType<Player>();
                if (player != null){
                    Vector2 teammatePos = new Vector2(player.transform.position.x, player.transform.position.z);
                    Vector2 aimLocation = ballInfo.SkillValues.AdjustedPassLocation(teammatePos, this);
                    Vector3 passTarget = new Vector3(aimLocation.x, 0f, aimLocation.y);
                    Pass(passTarget, 7, 1.75f);
                }
            }

            canBump = false;
        }

        //---- EVENT LISTENERS ----

        private void OnBump()
        {
            bumpAnimationTime = 0;
            canBump = true;
            animator.SetTrigger("Bump");
        }

        //---- GIZMOS ----

        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();

            Helpers.DrawTargetZone(targetZonePos, targetZoneSize, targetZoneColor, showTargetZone);
        }
    }
}
