using UnityEngine;
using KotB.Match;
using KotB.Actors;
using Cinemachine;

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
            foreach (TeamSO team in matchManager.MatchInfo.Teams) {
                team.Initialize();
                for (int i = 0; i < team.AthleteConfigs.Count; i++) {
                    Athlete athlete = InstantiateAthlete(team.AthleteConfigs[i], team, i);
                    team.AddAthlete(athlete);
                }
            }
        }

        private Athlete InstantiateAthlete(AthleteConfigSO athleteConfig, TeamSO team, int index) {
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
            materials[0] = athleteConfig.bottom != null ? athleteConfig.bottom.Mat : athleteConfig.skills.DefaultBottom.Mat;
            if (athleteConfig.outfit.ToString() != "NoShirt") materials[1] = athleteConfig.top != null ? athleteConfig.top.Mat : athleteConfig.skills.DefaultTop.Mat;
            r.materials = materials;

            // Move to Position
            Vector3 dPos = new Vector3(athlete.Skills.DefensePos.x * athlete.CourtSide, 0.01f, athlete.Skills.DefensePos.y * (index == 0 ? 1 : -1));
            if (athleteConfig.computerControlled) athleteGO.GetComponent<AI>().TargetPos = dPos;
            athleteGO.transform.position = dPos;

            // If Player and Cinemachine Serve Camera exists, assign
            if (!athleteConfig.computerControlled) {
                CinemachineVirtualCamera cam = GameObject.FindGameObjectWithTag("Serve Camera").GetComponent<CinemachineVirtualCamera>();
                if (cam != null) {
                    cam.Follow = athlete.transform;
                }
            }

            return athlete;
        }
    }
}
