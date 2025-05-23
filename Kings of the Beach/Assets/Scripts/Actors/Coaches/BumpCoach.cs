using UnityEngine;

namespace KotB.Actors
{
    public class BumpCoach : Coach
    {
        private enum BumpAim { Target, Pass }

        [Header("Bump Coach Settings")]
        [Tooltip("Choose Target to aim at the above target location or Pass to perform an AI pass with coach's accuracy")]
        [SerializeField] private BumpAim aim;
        [Tooltip("Select a teammate to pass to if using the Pass aim type")]
        [SerializeField] private Athlete passTeammate;

        protected override void PerformCoachAction()
        {
            if (aim == BumpAim.Target) {
                float posX = UnityEngine.Random.Range(targetZonePos.x - targetZoneSize.x / 2, targetZonePos.x + targetZoneSize.x / 2);
                float posY = UnityEngine.Random.Range(targetZonePos.y - targetZoneSize.y / 2, targetZonePos.y + targetZoneSize.y / 2);
                Pass(new Vector3(posX, 0, posY), 7, 1.75f);
            } else {
                if (passTeammate == null){
                    Debug.LogAssertion("No teammate selected but aim type was set to Pass. Please add a teammate to pass to.");
                    return;
                }
                Pass(CalculatePassTarget(passTeammate), 7, 1.75f);
            }
        }
    }
}
