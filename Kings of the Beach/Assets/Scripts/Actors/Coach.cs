using UnityEngine;

namespace KotB.Actors
{
    public class Coach : Athlete
    {
        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private Vector2 targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor;

        [Header("Game Input")]
        [SerializeField] private InputReader inputReader;

        private AI ai;

        protected override void Start() {
            base.Start();

            ai = FindObjectOfType<AI>();
            ai.Teammate = FindObjectOfType<Player>();

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
        }

        public void TakeBall() {
            ballInfo.GiveBall(this);
            ai.StateMachine.ChangeState(ai.DefenseState);
        }

        //---- EVENT LISTENERS ----

        private void OnBump()
        {
            float posX = Random.Range(targetZonePos.x - targetZoneSize.x / 2, targetZonePos.x + targetZoneSize.x / 2);
            float posY = Random.Range(targetZonePos.y - targetZoneSize.y / 2, targetZonePos.y + targetZoneSize.y / 2);
            Pass(new Vector3(posX, 0, posY));
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
