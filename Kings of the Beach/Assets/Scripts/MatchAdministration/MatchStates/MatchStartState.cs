using UnityEngine;
using KotB.Match;
using KotB.Actors;
using Cinemachine;
using KotB.Cinemachine;

namespace KotB.StatePattern.MatchStates
{
    public class MatchStartState : MatchBaseState
    {
        public MatchStartState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter()
        {
            InitializeTeams();
            matchManager.MatchInfo.Initialize();
        }

        private void InitializeTeams() {
            int teamIndex = 0;
            foreach (TeamSO team in matchManager.MatchInfo.Teams) {
                team.Initialize(teamIndex == 0 ? -1 : 1);
                for (int i = 0; i < team.AthleteConfigs.Count; i++) {
                    Athlete athlete = InstantiateAthlete(team.AthleteConfigs[i], team, i);
                    team.AddAthlete(athlete);
                }
                teamIndex++;
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
            athlete.SetStats(athleteConfig.athleteStats);

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

            // Assign Transition Profile
            athlete.StateMachine.SetActiveProfile(athleteConfig.computerControlled ? matchManager.AITransitionProfileName : matchManager.PlayerTransitionProfileName);

            // Move to Position
            Vector3 dPos = new Vector3(athlete.Skills.DefensePos.x * athlete.CourtSide, 0.01f, athlete.Skills.DefensePos.y * (index == 0 ? 1 : -1));
            if (athleteConfig.computerControlled) athleteGO.GetComponent<AI>().TargetPos = dPos;
            athleteGO.transform.position = dPos;

            // Reset Stats
            athlete.AthleteStats.ServeAttempts = 0;
            athlete.AthleteStats.ServeAces = 0;
            athlete.AthleteStats.ServeErrors = 0;
            athlete.AthleteStats.BlockAttempts = 0;
            athlete.AthleteStats.Blocks = 0;
            athlete.AthleteStats.BlockPoints = 0;
            athlete.AthleteStats.BlockErrors = 0;
            athlete.AthleteStats.Attacks = 0;
            athlete.AthleteStats.AttackKills = 0;
            athlete.AthleteStats.AttackErrors = 0;
            athlete.AthleteStats.Digs = 0;

            // If Player and Cinemachine Serve Camera exists, assign
            if (!athleteConfig.computerControlled) {
                GameObject go = GameObject.FindGameObjectWithTag("Serve Camera");
                if (go != null) {
                    CinemachineVirtualCamera cam = go.GetComponent<CinemachineVirtualCamera>();
                    if (cam != null) {
                        cam.Follow = athlete.transform;
                        Vector3 serveOffset = new Vector3(3, 3, 0);
                        CinemachineTransposer transposer = cam.GetComponent<CinemachineTransposer>();
                        if (transposer != null) {
                            transposer.m_FollowOffset = new Vector3(serveOffset.x * team.CourtSide.Value, serveOffset.y, serveOffset.z);
                        }
                        ServeCamDirection serveCamDirection = cam.GetComponent<ServeCamDirection>();
                        if (serveCamDirection != null) {
                            serveCamDirection.SetServeCamDir(team.CourtSide.Value);
                        }
                    }
                }
            }

            return athlete;
        }
    }
}
