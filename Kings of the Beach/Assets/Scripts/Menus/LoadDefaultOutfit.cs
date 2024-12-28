using UnityEngine;

namespace KotB.Menus
{
    public class LoadDefaultOutfit : MonoBehaviour
    {
        [SerializeField] private string resourcesPath;
        [SerializeField] private UIGroupSelect country;
        [SerializeField] private UIGroupSelect outfitTop;
        [SerializeField] private UIGroupSelect outfitBottom;

        private UIGroupSelect player;

        private void Awake() {
            player = GetComponent<UIGroupSelect>();
        }

        private void OnEnable() {
            player.SelectionChanged += OnSelectionChanged;
        }

        private void OnDisable() {
            player.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged() {
            string folderPath = resourcesPath + $"/Male/{country.GetSelectedValue()}";
            SkillsSO[] skills = Resources.LoadAll<SkillsSO>(folderPath);
            for(int i = 0; i < skills.Length; i++) {
                if (skills[i].name == player.GetSelectedValue()) {
                    outfitTop.MenuText.text = skills[i].DefaultTop.name;
                    outfitBottom.MenuText.text = skills[i].DefaultBottom.name;
                    break;
                }
            }
        }
    }
}
