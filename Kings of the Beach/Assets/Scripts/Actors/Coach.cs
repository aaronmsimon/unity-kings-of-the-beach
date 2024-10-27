using UnityEngine;
using KotB.StatePattern.MatchStates;
using KotB.Match;

namespace KotB.Actors
{
    public enum CoachType {
        Pass,
        Spike
    }

    public class Coach : Athlete
    {
        [Header("Coach Info")]
        [SerializeField] private CoachType coachType;

        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private Vector2 targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor;

        [Header("Game Input")]
        [SerializeField] private InputReader inputReader;

        private AI ai;
        private Player player;
        private MatchManager matchManager;

        private float bumpFrames = 7;
        private float bumpAnimationTime;
        private bool canBump;

        protected override void Start() {
            base.Start();

            player = FindObjectOfType<Player>();

            if (coachType == CoachType.Pass) {
                ai = FindObjectOfType<AI>();
                if (ai != null)
                    ai.Teammate = player;
            }

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
            if (ai != null && coachType == CoachType.Pass) {
                ai.StateMachine.ChangeState(ai.DefenseState);
            }
            player.StateMachine.ChangeState(player.NormalState);
            transform.forward = Vector3.right * -CourtSide;
            animator.Play("HoldBall");
        }

        private void Bump() {
            float posX = Random.Range(targetZonePos.x - targetZoneSize.x / 2, targetZonePos.x + targetZoneSize.x / 2);
            float posY = Random.Range(targetZonePos.y - targetZoneSize.y / 2, targetZonePos.y + targetZoneSize.y / 2);
            Pass(new Vector3(posX, 0, posY));
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

            DrawTargetZone();
        }

        private void DrawTargetZone() {
            float targetZoneHeight = 0.1f;

            Vector3[] points;
            points = new Vector3[8] {
                new Vector3(targetZonePos.x - targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y + targetZoneSize.y / 2),
                new Vector3(targetZonePos.x + targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y + targetZoneSize.y / 2),
                new Vector3(targetZonePos.x + targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y + targetZoneSize.y / 2),
                new Vector3(targetZonePos.x + targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y - targetZoneSize.y / 2),
                new Vector3(targetZonePos.x + targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y - targetZoneSize.y / 2),
                new Vector3(targetZonePos.x - targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y - targetZoneSize.y / 2),
                new Vector3(targetZonePos.x - targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y - targetZoneSize.y / 2),
                new Vector3(targetZonePos.x - targetZoneSize.x / 2, targetZoneHeight, targetZonePos.y + targetZoneSize.y / 2)
            };

            Gizmos.color = targetZoneColor;
            if (showTargetZone)
                Gizmos.DrawLineList(points);
        }
    }
}
