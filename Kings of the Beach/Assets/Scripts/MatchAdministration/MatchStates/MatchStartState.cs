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
            matchManager.MatchInfo.Initialize();

            matchManager.StateMachine.ChangeState(matchManager.PrePointState);
        }

        private void InitializeTeams() {
            int teamIndex = 0;

            foreach (TeamSO team in matchManager.MatchInfo.Teams) {
                team.CourtSide.Value = teamIndex == 0 ? -1 : 1;
                Debug.Log(team.AthleteConfigs.Count);
                foreach (AthleteConfigSO athleteConfig in team.AthleteConfigs) {
                    Athlete athlete = InstantiateAthlete(athleteConfig, team);
                    team.AddAthlete(athlete);
                }
            }
        }

        private Athlete InstantiateAthlete(AthleteConfigSO athleteConfig, TeamSO team) {
            // Instantiate Prefab
            GameObject athletePrefab = athleteConfig.computerControlled ? matchManager.AIPrefab : matchManager.PlayerPrefab;
            GameObject athleteGO = GameObject.Instantiate(athletePrefab);
            Athlete athlete = athleteGO.GetComponent<Athlete>();

            // Assign Scriptable Objects
            athlete.SetSkills(athleteConfig.skills);
            athlete.SetCourtSide(team.CourtSide);

            // Activate Outfit
            string outfit = athleteConfig.outfit.ToString() ?? athleteConfig.skills.DefaultOutfit.ToString();
            Transform outfitTransform = athleteGO.transform.Find("Volleyball-Character").Find($"Volleyball-{athleteConfig.skills.Gender}-{outfit}");
            outfitTransform.gameObject.SetActive(true);

            // Assign Materials
            SkinnedMeshRenderer r = outfitTransform.GetComponent<SkinnedMeshRenderer>();
            Material[] materials = r.materials;
            materials[0] = athleteConfig.bottom != null ? athleteConfig.bottom : athleteConfig.skills.DefaultBottom;
            if (athleteConfig.outfit.ToString() != "NoShirt") materials[1] = athleteConfig.top != null ? athleteConfig.top : athleteConfig.skills.DefaultTop;
            r.materials = materials;

            // Move to Position
            Vector2 dPos = new Vector3(athlete.Skills.DefensePos.x * athlete.CourtSide, 0.01f, athlete.Skills.DefensePos.y);
            if (athleteConfig.computerControlled) athleteGO.GetComponent<AI>().TargetPos = dPos;
            athleteGO.transform.position = dPos;

            return athlete;
        }
    }
}
