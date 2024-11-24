using KotB.Actors;
using UnityEngine;

namespace KotB.Testing
{
    public class AttackArea : MonoBehaviour
    {
        [SerializeField] private AI ai;
        [SerializeField] private Athlete opponent1;
        [SerializeField] private Athlete opponent2;
        [SerializeField] private Color deepZoneColor;
        [SerializeField] private Color shallowZoneColor;

        private void CalculateZones() {
            Vector3 deepOpponent = (Mathf.Max(Mathf.Abs(opponent1.transform.position.x), Mathf.Abs(opponent2.transform.position.x)) == Mathf.Abs(opponent1.transform.position.x) ? opponent1 : opponent2).transform.position;
            Vector3 shallowOpponent = (Mathf.Max(Mathf.Abs(opponent1.transform.position.x), Mathf.Abs(opponent2.transform.position.x)) == Mathf.Abs(opponent1.transform.position.x) ? opponent2 : opponent1).transform.position;

            // Deep Zone
            Vector2 deepZonePos = new Vector2(
                ((ai.CourtSideLength - Mathf.Abs(shallowOpponent.x)) / 2 + Mathf.Abs(shallowOpponent.x)) * Mathf.Sign(deepOpponent.x),
                ((Mathf.Abs(deepOpponent.z) + 4) / 2 - Mathf.Abs(deepOpponent.z)) * -Mathf.Sign(deepOpponent.z)
            );
            Vector2 deepZoneSize = new Vector2(
                ai.CourtSideLength - shallowOpponent.x,
                Mathf.Abs(deepOpponent.z) + 4
            );
            Helpers.DrawTargetZone(deepZonePos, deepZoneSize, deepZoneColor, true);

            // Shallow Zone
            Vector2 shallowZonePos = new Vector2(
                Mathf.Abs(deepOpponent.x) / 2 * Mathf.Sign(deepOpponent.x),
                ((Mathf.Abs(shallowOpponent.z) + 4) / 2 - Mathf.Abs(shallowOpponent.z)) * -Mathf.Sign(shallowOpponent.z)
            );
            Vector2 shallowZoneSize = new Vector2(
                Mathf.Abs(deepOpponent.x),
                Mathf.Abs(shallowOpponent.z) + 4
            );
            Helpers.DrawTargetZone(shallowZonePos, shallowZoneSize, shallowZoneColor, true);
        }

        //---- GIZMOS ----

        private void OnDrawGizmos() {
            CalculateZones();
        }
    }
}
