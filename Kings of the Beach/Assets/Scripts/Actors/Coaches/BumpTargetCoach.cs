using UnityEngine;
using KotB.Items;

namespace KotB.Actors
{
    public class BumpTargetCoach : CoachAction
    {
        [SerializeField] private PassType passType = PassType.Bump;

        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private Vector2 targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor = Color.red;

        public override void Execute()
        {
            float posX = Random.Range(targetZonePos.x - targetZoneSize.x / 2, targetZonePos.x + targetZoneSize.x / 2);
            float posY = Random.Range(targetZonePos.y - targetZoneSize.y / 2, targetZonePos.y + targetZoneSize.y / 2);
            coach.Pass(new Vector3(posX, 0.01f, posY), 7, 1.75f, passType);
        }

        protected void OnDrawGizmos() {
            Helpers.DrawTargetZone(targetZonePos, targetZoneSize, targetZoneColor, showTargetZone);
        }
    }
}
