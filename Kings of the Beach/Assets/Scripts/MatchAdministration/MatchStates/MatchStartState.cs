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
            InitializeTeams();

            matchManager.StateMachine.ChangeState(matchManager.PrePointState);
        }

        private void InitializeTeams() {
            int teamIndex = 0;

            foreach (TeamSO team in matchManager.MatchInfo.Teams) {
                team.CourtSide.Value = teamIndex == 0 ? -1 : 1;
                foreach (AthleteConfig athleteConfig in team.AthleteConfigs) {
                    Athlete athlete = InstantiateAthlete(athleteConfig, team);
                    team.AddAthlete(athlete);
                }
            }
        }

        private Athlete InstantiateAthlete(AthleteConfig athleteConfig, TeamSO team) {
            // Instantiate Prefab
            GameObject athletePrefab = athleteConfig.ComputerControlled ? matchManager.AIPrefab : matchManager.PlayerPrefab;
            GameObject athleteGO = GameObject.Instantiate(athletePrefab);
            Athlete athlete = athleteGO.GetComponent<Athlete>();

            // Assign Scriptable Objects
            athlete.SetSkills(athleteConfig.Skills);
            athlete.SetCourtSide(team.CourtSide);

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
            Vector2 dPos = new Vector3(athlete.Skills.DefensePos.x * athlete.CourtSide, 0.01f, athlete.Skills.DefensePos.y);
            if (athleteConfig.ComputerControlled) athleteGO.GetComponent<AI>().TargetPos = dPos;
            athleteGO.transform.position = dPos;

            return athlete;
        }
    }
}
