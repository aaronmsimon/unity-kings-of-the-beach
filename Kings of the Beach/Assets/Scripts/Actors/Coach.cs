using UnityEngine;

namespace KotB.Actors
{
    public abstract class Coach : Athlete
    {
        [Header("Target Area")]
        [SerializeField] protected Vector2 targetZonePos;
        [SerializeField] protected Vector2 targetZoneSize;
        [SerializeField] protected bool showTargetZone;
        [SerializeField] protected Color targetZoneColor = Color.red;

        protected override void Start() {
            base.Start();

            Reset();
        }

        protected virtual void Reset() {
        }

        public abstract void CoachAction();
        
        protected override void OnDrawGizmos() {
            base.OnDrawGizmos();

            Helpers.DrawTargetZone(targetZonePos, targetZoneSize, targetZoneColor, showTargetZone);
        }
    }
}
