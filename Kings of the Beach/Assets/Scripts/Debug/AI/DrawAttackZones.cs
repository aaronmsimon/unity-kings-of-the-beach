using System.Collections.Generic;
using UnityEngine;
using KotB.Actors;
using Cackenballz.Helpers;

namespace KotB.Testing
{
    public class DrawAttackZones : MonoBehaviour
    {
        [SerializeField][Range(1,10)] private float skillLevel;
        [SerializeField][Range(0,1)] private float rand;

        private AI ai;
        private Vector2[] attackZoneCenters = new Vector2[2];
        private Vector2[] attackZoneSizes = new Vector2[2];

        private void Awake() {
            ai = GetComponent<AI>();
        }

        private void Update() {
            CalculateAttackZones();
        }

        private void CalculateAttackZones() {
            List<Athlete> opponents = ai.MatchInfo.GetOpposingTeam(ai).Athletes;
            Athlete deepOpponent = Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[0] : opponents[1];
            Athlete shallowOpponent = Mathf.Max(Mathf.Abs(opponents[0].transform.position.x), Mathf.Abs(opponents[1].transform.position.x)) == Mathf.Abs(opponents[0].transform.position.x) ? opponents[1] : opponents[0];
            Vector3 deepPos = deepOpponent.transform.position;
            Vector3 shallowPos = shallowOpponent.transform.position;
            
            // Deep Zone
            attackZoneCenters[0] = new Vector2(
                ((ai.CourtSideLength - Mathf.Abs(shallowPos.x)) / 2 + Mathf.Abs(shallowPos.x)) * Mathf.Sign(deepPos.x),
                ((Mathf.Abs(deepPos.z) + 4) / 2 - Mathf.Abs(deepPos.z)) * -Mathf.Sign(deepPos.z)
            );
            // float deepDecrease = WeightedDecrease(skillLevel);
            float deepDecrease = Mathf.Pow(rand, skillLevel / 2);
            float shallowDecrease = deepDecrease;
            attackZoneSizes[0] = new Vector2(
                (ai.CourtSideLength - Mathf.Abs(shallowPos.x)) * (1 - deepDecrease),
                (Mathf.Abs(deepPos.z) + 4) * (1 - deepDecrease)
            );

            // Shallow Zone
            attackZoneCenters[1] = new Vector2(
                Mathf.Abs(deepPos.x) / 2 * Mathf.Sign(deepPos.x),
                ((Mathf.Abs(shallowPos.z) + 4) / 2 - Mathf.Abs(shallowPos.z)) * -Mathf.Sign(shallowPos.z)
            );
            // float shallowDecrease = WeightedDecrease(skillLevel);
            attackZoneSizes[1] = new Vector2(
                Mathf.Abs(deepPos.x) * (1 - shallowDecrease),
                (Mathf.Abs(shallowPos.z) + 4) * (1 - shallowDecrease)
            );
        }

        private int LargestAttackZoneIndex() {
            float largestArea = -1;
            int index = -1;

            for (int i = 0; i < attackZoneSizes.Length; i++)
            {
                float thisArea = attackZoneSizes[i].x * attackZoneSizes[i].y;
                if (thisArea > largestArea) {
                    largestArea = thisArea;
                    index = i;
                }                
            }

            return index;
        }

        private float WeightedDecrease(float skill) {
            return Mathf.Pow(Random.value, skill / 2);
        }

        private void OnDrawGizmos() {
            int largestZoneIndex = LargestAttackZoneIndex();
            // Primary (largest)
            GizmoHelpers.DrawGizmoRectangle(
                new Vector3(attackZoneCenters[largestZoneIndex].x, 0.01f,
                attackZoneCenters[largestZoneIndex].y),
                attackZoneSizes[largestZoneIndex].x,
                attackZoneSizes[largestZoneIndex].y, Color.red
            );
            // Secondary (smallest)
            GizmoHelpers.DrawGizmoRectangle(
                new Vector3(attackZoneCenters[1 - largestZoneIndex].x, 0.01f,
                attackZoneCenters[1 - largestZoneIndex].y),
                attackZoneSizes[1 - largestZoneIndex].x,
                attackZoneSizes[1 - largestZoneIndex].y, Color.green
            );
        }
    }
}
