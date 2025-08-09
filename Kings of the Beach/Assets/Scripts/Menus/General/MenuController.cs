using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using KotB.Menus;

namespace MenuSystem
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private List<MenuGroup> menuGroups;
        [SerializeField] private InputReader inputReader;

        

        private int menuGroupIndex = 0;
        private Label[] labels;

        private void Awake() {
            LoadMenus();
            inputReader.EnableMenuInput();
        }

        private void OnEnable() {
            inputReader.selectionUpEvent += OnSelectionUp;
            inputReader.selectionDownEvent += OnSelectionDown;
            inputReader.selectionLeftEvent += OnSelectionLeft;
            inputReader.selectionRightEvent += OnSelectionRight;
        }

        private void OnDisable() {
            inputReader.selectionUpEvent -= OnSelectionUp;
            inputReader.selectionDownEvent -= OnSelectionDown;
            inputReader.selectionLeftEvent -= OnSelectionLeft;
            inputReader.selectionRightEvent -= OnSelectionRight;
        }

        public void LoadMenus() {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement menuContainer = root.Query<VisualElement>("Menu-Container");

            labels = new Label[menuGroups.Count];

            int index = 0;
            foreach (MenuGroup menuGroup in menuGroups) {
                int sfIndex = menuGroup.SubfolderIndex;
                menuGroup.LoadOptions(sfIndex > -1 ? menuGroups[sfIndex].Text : null);

                Label label = new Label(menuGroup.Text);
                label.name = menuGroup.Text.Replace(" ","-");
                label.AddToClassList("menu-list-selection");
                menuContainer.Add(label);

                labels[index] = label;
                index++;
            }
        }

        private void OnSelectionUp() {
            IncrementMenuIndex(-1);
        }

        private void OnSelectionDown() {
            IncrementMenuIndex(1);
        }

        private void OnSelectionLeft() {
            menuGroups[menuGroupIndex].IncrementItemIndex(1);
            UpdateDisplay();
        }

        private void OnSelectionRight() {
            menuGroups[menuGroupIndex].IncrementItemIndex(-1);
            UpdateDisplay();
        }

        private void UpdateDisplay() {
            if (menuGroups.Count > 0) {
                labels[menuGroupIndex].text = menuGroups[menuGroupIndex].Text;
            }
        }

        private void IncrementMenuIndex(int direction) {
            if (direction > 0) {
                if (menuGroupIndex < menuGroups.Count - 1) {
                    menuGroupIndex++;
                } else {
                    menuGroupIndex = 0;
                }
            } else {
                if (menuGroupIndex > 0) {
                    menuGroupIndex--;
                } else {
                    menuGroupIndex = menuGroups.Count - 1;
                }
            }
        }
    }
}
