using UnityEngine;
using System.Linq;
using KotB.Match;
using KotB.Actors;

namespace KotB.StatePattern.MatchStates
{
    public class MatchStartState : MatchBaseState
    {
        public MatchStartState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter()
        {
            InitializeTeams();

            matchManager.StateMachine.ChangeState(matchManager.PrePointState);
        }

        private void InitializeTeams() {
            // matchManager.MatchInfo.InitializeTeamList();
            // for (int i = 0; i < matchManager.TeamConfigs.Count; i++) {
            //     int courtSide = i == 0 ? -1 : 1;
            //     TeamSO team = new Team(matchManager.TeamConfigs[i].TeamName, matchManager.TeamConfigs[i].Score, courtSide);
            //     foreach (AthleteConfig athleteConfig in matchManager.TeamConfigs[i].AthleteConfigs) {
            //         Athlete athlete = InstantiateAthlete(athleteConfig, courtSide);
            //         team.AddAthlete(athlete);
            //     }
            //     matchManager.MatchInfo.Teams.Add(team);
            // }
        }

        private Athlete InstantiateAthlete(AthleteConfig athleteConfig, int courtSide) {
            // Instantiate Prefab
            GameObject athletePrefab = athleteConfig.ComputerControlled ? matchManager.AIPrefab : matchManager.PlayerPrefab;
            GameObject athleteGO = GameObject.Instantiate(athletePrefab);

            // Assign Skills
            athleteGO.GetComponent<Athlete>().SetSkills(athleteConfig.Skills);

            // Activate Outfit
            string outfit = athleteConfig.Outfit.ToString() ?? athleteConfig.Skills.DefaultOutfit.ToString();
            Transform outfitTransform = athleteGO.transform.Find("Volleyball-Character").Find($"Volleyball-{athleteConfig.Skills.Gender}-{outfit}");
            outfitTransform.gameObject.SetActive(true);

            // Assign Materials
            SkinnedMeshRenderer r = outfitTransform.GetComponent<SkinnedMeshRenderer>();
            Material[] materials = r.materials;
            materials[0] = athleteConfig.Bottom != null ? athleteConfig.Bottom : athleteConfig.Skills.DefaultBottom;
            if (athleteConfig.Outfit.ToString() != "NoShirt") materials[1] = athleteConfig.Top != null ? athleteConfig.Top : athleteConfig.Skills.DefaultTop;
            r.materials = materials;

            // Move to Position
            Vector2 dPos = new Vector3(athleteGO.GetComponent<Athlete>().Skills.DefensePos.x * courtSide, 0.01f, athleteGO.GetComponent<Athlete>().Skills.DefensePos.y);
            if (athleteConfig.ComputerControlled) athleteGO.GetComponent<AI>().TargetPos = dPos;
            athleteGO.transform.position = dPos;

            return athleteGO.GetComponent<Athlete>();
        }
    }
}
