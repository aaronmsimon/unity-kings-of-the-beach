using UnityEngine;
using KotB.Match;
using KotB.StatePattern.MatchStates;

namespace KotB.Actors
{
    public class Coach : MonoBehaviour
    {
        [Header("Coaching Details")]
        [SerializeField] private float ballHeight;
        [SerializeField] private float ballDuration;

        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private float targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor;

        [Header("Game Input")]
        [SerializeField] private InputReader inputReader;

        [Header("temp")]
        [SerializeField] private Player player;
        [SerializeField] private float speed;
        [SerializeField] private MatchInfoSO matchInfo;

        private Bump bump;

        private void Awake() {
            bump = GetComponent<Bump>();

            // place ball at same start
            TakeBall();

            // place player at defensive position
            player.transform.position = new Vector3(player.Skills.ServingPartnerPos.x * player.CourtSide, player.Skills.ServingPartnerPos.y, player.Skills.ServingPartnerPos.z);

            InPlayState inPlayState = new InPlayState(null);
            matchInfo.CurrentState = inPlayState;
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable()
        {
            inputReader.testEvent += OnBump;
        }
        
        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable()
        {
            inputReader.testEvent -= OnBump;
        }

        private void Update() {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                TakeBall();
                player.StateMachine.ChangeState(player.NormalState);
            }
            #endif
            Debug.Log(matchInfo.CurrentState);
        }

        public void TakeBall() {
            bump.ball.transform.position = new Vector3(transform.position.x, 1.09f, transform.position.z);
        }

        //---- EVENT LISTENERS ----

        private void OnBump()
        {
            float posX = Random.Range(targetZonePos.x - targetZoneSize / 2, targetZonePos.x + targetZoneSize / 2);
            float posY = Random.Range(targetZonePos.y - targetZoneSize / 2, targetZonePos.y + targetZoneSize / 2);
            bump.PerformBump(new Vector3(posX, 0, posY), ballHeight, ballDuration);
        }

        //---- GIZMOS ----

        private void OnDrawGizmos() {
            DrawTargetZone();
        }

        private void DrawTargetZone() {
            float targetZoneHeight = 0.1f;

            Vector3[] points;
            points = new Vector3[8] {
                new Vector3(targetZonePos.x - targetZoneSize / 2, targetZoneHeight, targetZonePos.y + targetZoneSize / 2),
                new Vector3(targetZonePos.x + targetZoneSize / 2, targetZoneHeight, targetZonePos.y + targetZoneSize / 2),
                new Vector3(targetZonePos.x + targetZoneSize / 2, targetZoneHeight, targetZonePos.y + targetZoneSize / 2),
                new Vector3(targetZonePos.x + targetZoneSize / 2, targetZoneHeight, targetZonePos.y - targetZoneSize / 2),
                new Vector3(targetZonePos.x + targetZoneSize / 2, targetZoneHeight, targetZonePos.y - targetZoneSize / 2),
                new Vector3(targetZonePos.x - targetZoneSize / 2, targetZoneHeight, targetZonePos.y - targetZoneSize / 2),
                new Vector3(targetZonePos.x - targetZoneSize / 2, targetZoneHeight, targetZonePos.y - targetZoneSize / 2),
                new Vector3(targetZonePos.x - targetZoneSize / 2, targetZoneHeight, targetZonePos.y + targetZoneSize / 2)
            };

            Gizmos.color = targetZoneColor;
            if (showTargetZone)
                Gizmos.DrawLineList(points);
        }
    }
}
