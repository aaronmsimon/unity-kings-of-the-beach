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
            PopulateTeams();

            matchManager.StateMachine.ChangeState(matchManager.PrePointState);
        }

        private void PopulateTeams() {
            foreach (Team team in matchManager.MatchInfo.Teams) {
                foreach (AthleteInfo athleteInfo in team.TeamInfo.AthleteInfos) {
                    Athlete athlete = InstantiateAthlete(athleteInfo, team.TeamInfo.CourtSide);
                //     team.AssignAthlete(athlete);
                }
            }
        }

        private Athlete InstantiateAthlete(AthleteInfo athleteInfo, int courtSide) {
            // Instantiate Prefab
            GameObject athletePrefab = athleteInfo.ComputerControlled ? matchManager.AIPrefab : matchManager.PlayerPrefab;
            GameObject athleteGO = GameObject.Instantiate(athletePrefab);

            // Assign Skills
            athleteGO.GetComponent<Athlete>().SetSkills(athleteInfo.Skills);

            // Activate Outfit
            string outfit = athleteInfo.Outfit.ToString() ?? athleteInfo.Skills.DefaultOutfit.ToString();
            Transform outfitTransform = athleteGO.transform.Find("Volleyball-Character").Find($"Volleyball-{athleteInfo.Skills.Gender}-{outfit}");
            outfitTransform.gameObject.SetActive(true);

            // Assign Materials
            SkinnedMeshRenderer r = outfitTransform.GetComponent<SkinnedMeshRenderer>();
            Material[] materials = r.materials;
            materials[0] = athleteInfo.Bottom != null ? athleteInfo.Bottom : athleteInfo.Skills.DefaultBottom;
            if (athleteInfo.Outfit.ToString() != "NoShirt") materials[1] = athleteInfo.Top != null ? athleteInfo.Top : athleteInfo.Skills.DefaultTop;
            r.materials = materials;

            // Move to Position
            Vector2 dPos = new Vector3(athleteGO.GetComponent<Athlete>().Skills.DefensePos.x * courtSide, 0.01f, athleteGO.GetComponent<Athlete>().Skills.DefensePos.y);
            if (athleteInfo.ComputerControlled) athleteGO.GetComponent<AI>().TargetPos = dPos;
            athleteGO.transform.position = dPos;

            return athleteGO.GetComponent<Athlete>();
        }
    }
}
