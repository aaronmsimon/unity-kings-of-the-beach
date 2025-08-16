using UnityEngine;
using MenuSystem;

namespace KotB.Menus
{
    public class LoadDefaultOutfit : MonoBehaviour
    {
        private MenuController menuController;

        private void Awake() {
            menuController = GetComponent<MenuController>();
            if (menuController == null) {
                Debug.LogAssertion("Menu Controller component not found.");
            }
        }

        private void OnEnable() {
            menuController.GetCurrentMenuGroup().SelectionChanged += OnSelectionChanged;
        }

        private void OnDisable() {
            menuController.GetCurrentMenuGroup().SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged() {
            MenuGroup menuGroup = menuController.GetCurrentMenuGroup();
            if (menuGroup.MenuGroupName != "Athlete") return;

            SkillsSO[] skills = Resources.LoadAll<SkillsSO>(menuGroup.ResourcesPath);
            for(int i = 0; i < skills.Length; i++) {
                if (skills[i].name == menuGroup.Text) {
                    // outfitTop.MenuText.text = skills[i].DefaultTop.name;
                    // outfitBottom.MenuText.text = skills[i].DefaultBottom.name;
                    break;
                }
            }
        }
    }
}
