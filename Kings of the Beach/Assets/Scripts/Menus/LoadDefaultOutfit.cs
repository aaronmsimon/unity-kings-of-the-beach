using UnityEngine;
using MenuSystem;
using System.Linq;

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
            menuController.GetMenuGroupByName("Athletes").SelectionChanged += OnSelectionChanged;
        }

        private void OnDisable() {
            menuController.GetMenuGroupByName("Athletes").SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged() {
            MenuGroup menuGroup = menuController.GetMenuGroupByName("Athletes");
            int athleteIndex = menuController.GetMenuGroupIndexByName("Athletes");

            SkillsSO[] skills = Resources.LoadAll<SkillsSO>(menuGroup.ResourcesPath);
            SkillsSO athleteSkills = skills.FirstOrDefault(s => s.name == menuGroup.Text);

            MenuGroup outfitTop = menuController.GetMenuGroupByName("Outfit Top");
            int topIndex = outfitTop.GetIndexByName(athleteSkills.DefaultTop.name);
            Debug.Log($"athlete index: {athleteIndex}, top index: {topIndex}");
            // menuController.SetSelectionIndexByGroup(athleteIndex, topIndex);

            // MenuGroup outfitBottom = menuController.GetMenuGroupByName("Outfit Bottom");
            // int bottomIndex = outfitBottom.GetIndexByName(athleteSkills.DefaultBottom.name);
            // menuController.SetSelectionIndexByGroup(athleteIndex, bottomIndex);
        }
    }
}
