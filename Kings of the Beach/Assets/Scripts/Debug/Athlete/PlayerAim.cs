using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.PlayerStates;
using Cackenballz.Helpers;

namespace KotB.Testing
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private float targetSize;
        [SerializeField] private Color targetColor;
        [SerializeField] private float skill;
        [SerializeField] private float timingVar;

        private Player player;
        private Vector3 targetPos;

        private void Awake() {
            player = GetComponent<Player>();
        }

        private void Update() {
            if (player.StateMachine.CurrentState is LockState) {
                SetTargetPos(false);
            }
        }

        private void SetTargetPos(bool pass) {
            Vector2 aim = Helpers.CircleMappedToSquare(player.MoveInput.x, player.MoveInput.y);

            float targetX = aim.x * 5 + 4 * (pass ? player.CourtSide : -player.CourtSide);
            float targetZ = aim.y * 5;
            targetPos = new Vector3(targetX, 0f, targetZ);
        }

        private void OnDrawGizmos() {
            GizmoHelpers.DrawGizmoCircle(targetPos, targetSize, targetColor, 8);
            float skillFactor = Mathf.Lerp(2f, 0.2f, (skill - 1f) / 9f);
            float radius = timingVar * skillFactor;
            GizmoHelpers.DrawGizmoCircle(targetPos, radius, targetColor, 8);
        }
    }
}
