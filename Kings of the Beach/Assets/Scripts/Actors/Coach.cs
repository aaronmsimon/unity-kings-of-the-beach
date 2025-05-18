using UnityEngine;

namespace KotB.Actors
{
    public abstract class Coach : Athlete
    {
        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private Vector2 targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor = Color.red;

        public abstract void CoachAction();
        
        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();

            Helpers.DrawTargetZone(targetZonePos, targetZoneSize, targetZoneColor, showTargetZone);
        }
    }
}
