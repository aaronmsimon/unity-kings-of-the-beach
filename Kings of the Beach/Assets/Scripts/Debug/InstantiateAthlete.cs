using UnityEngine;
using KotB.Actors;

namespace KotB.Testing
{
    public class InstantiateAthlete : MonoBehaviour
    {
        [SerializeField] private AthleteInfo athleteInfo;
        [SerializeField] private GameObject aiPrefab;
        [SerializeField] private GameObject playerPrefab;

        private void Start() {
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
            Vector2 dPos = athleteGO.GetComponent<Athlete>().Skills.DefensePos;
            athleteGO.transform.position = new Vector3(dPos.x, 0.01f, dPos.y);
        }
    }
}
