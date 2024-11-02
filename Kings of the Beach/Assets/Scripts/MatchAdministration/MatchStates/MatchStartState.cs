using UnityEngine;
using KotB.Match;
using KotB.Actors;

namespace KotB.StatePattern.MatchStates
{
    public class MatchStartState : MatchBaseState
    {
        public MatchStartState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter()
        {
            // Instantiate and assign all Athletes
            foreach (Team team in matchManager.MatchInfo.Teams) {
                foreach (AthleteInfo athleteInfo in team.AthletesInfo) {
                    Athlete athlete = InstantiateAthlete(athleteInfo);
                    team.AssignAthlete(athlete);
                }
            }

            matchManager.StateMachine.ChangeState(matchManager.PrePointState);
        }

        private Athlete InstantiateAthlete(AthleteInfo athleteInfo) {
            GameObject athletePrefab = athleteInfo.ComputerControlled ? matchManager.AIPrefab : matchManager.PlayerPrefab;
            GameObject athleteGO = GameObject.Instantiate(athletePrefab);
            athleteGO.GetComponent<Athlete>().SetSkills(athleteInfo.Skills);
            Transform outfit = athleteGO.transform.Find("Volleyball-Character").Find($"Volleyball-{athleteInfo.Skills.Gender}-{athleteInfo.Outfit}");
            outfit.gameObject.SetActive(true);
            SkinnedMeshRenderer r = outfit.GetComponent<SkinnedMeshRenderer>();
            Material[] materials = r.materials;
            materials[0] = athleteInfo.Bottom;
            if (athleteInfo.Outfit.ToString() != "NoShirt") materials[1] = athleteInfo.Top;
            r.materials = materials;
            Vector2 dPos = athleteGO.GetComponent<Athlete>().Skills.DefensePos;
            athleteGO.transform.position = new Vector3(dPos.x, 0.01f, dPos.y);
            return athleteGO.GetComponent<Athlete>();
        }
    }
}
