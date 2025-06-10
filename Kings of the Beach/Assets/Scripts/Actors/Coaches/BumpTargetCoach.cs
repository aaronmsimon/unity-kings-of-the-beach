using UnityEngine;

namespace KotB.Actors
{
    [System.Serializable]
    public class BumpTargetCoach : CoachAction
    {
        [Header("Target Area")]
        [SerializeField] private Vector2 targetZonePos;
        [SerializeField] private Vector2 targetZoneSize;
        [SerializeField] private bool showTargetZone;
        [SerializeField] private Color targetZoneColor = Color.red;

        public BumpTargetCoach(Vector2 target)
        {
            targetZonePos = target;
        }

        public override void Execute(Coach coach)
        {
            float posX = UnityEngine.Random.Range(targetZonePos.x - targetZoneSize.x / 2, targetZonePos.x + targetZoneSize.x / 2);
            float posY = UnityEngine.Random.Range(targetZonePos.y - targetZoneSize.y / 2, targetZonePos.y + targetZoneSize.y / 2);
            coach.Pass(new Vector3(posX, 0, posY), 7, 1.75f);
        }

        // protected override void OnDrawGizmos() {
        //     base.OnDrawGizmos();

        //     Helpers.DrawTargetZone(targetZonePos, targetZoneSize, targetZoneColor, showTargetZone);
        // }
    }
}
