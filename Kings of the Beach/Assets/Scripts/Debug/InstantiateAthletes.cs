using UnityEngine;
using KotB.Actors;
using KotB.Match;

namespace KotB.Testing
{
    public class InstantiateAthletes : MonoBehaviour
    {
        [SerializeField] private SkillsSO[] skills;
        [SerializeField] private int playerIndex;
        [SerializeField] private GameObject aiPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private MatchInfoSO matchInfo;

        private void Start() {
            for (int i = 0; i < skills.Length; i++) {
                // AddAthlete(new AthleteInfo(skills[i], i != playerIndex, skills[i].DefaultOutfit, skills[i].DefaultTop, skills[i].DefaultBottom));
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.B)) {
                // Debug.Log(matchInfo.)
            }
        }

        private void AddAthlete(AthleteInfo athleteInfo) {
            GameObject athletePrefab = athleteInfo.ComputerControlled ? aiPrefab : playerPrefab;
            GameObject athleteGO = Instantiate(athletePrefab);
            athleteGO.GetComponent<Athlete>().SetSkills(athleteInfo.Skills);
            Transform outfit = athleteGO.transform.Find("Volleyball-Character").Find($"Volleyball-{athleteInfo.Skills.Gender}-{athleteInfo.Outfit}");
            outfit.gameObject.SetActive(true);
            SkinnedMeshRenderer r = outfit.GetComponent<SkinnedMeshRenderer>();
            Material[] materials = r.materials;
            materials[0] = athleteInfo.Bottom;
            if (athleteInfo.Outfit.ToString() != "NoShirt") materials[1] = athleteInfo.Top;
            r.materials = materials;
            Vector3 dPos = new Vector3(athleteGO.GetComponent<Athlete>().Skills.DefensePos.x, 0.01f, athleteGO.GetComponent<Athlete>().Skills.DefensePos.y);
            Debug.Log(athleteInfo.Skills.AthleteName + " " + dPos);
            if (athleteInfo.ComputerControlled) athleteGO.GetComponent<AI>().TargetPos = dPos;
            athleteGO.transform.position = dPos;
        }
    }
}
