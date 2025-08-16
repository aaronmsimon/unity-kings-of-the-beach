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

        private VisualElement root;
        private VisualElement menuContainer;

        private int menuGroupIndex = 0;
        private Label[] labels;
        private Dictionary<int, List<int>> dependencyMap;

        private void Awake() {
            LoadMenus();
            BuildDependencyMap();
            
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

        public MenuGroup GetCurrentMenuGroup() {
            return menuGroups[menuGroupIndex];
        }

        private void LoadMenus() {
            root = GetComponent<UIDocument>().rootVisualElement;
            menuContainer = root.Query<VisualElement>("Menu-Container");

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

            UpdateDisplay();
        }

        private void BuildDependencyMap() {
            dependencyMap = new Dictionary<int, List<int>>();
            
            for (int i = 0; i < menuGroups.Count; i++)
            {
                int p = menuGroups[i].SubfolderIndex;
                if (p >= 0 && p < menuGroups.Count)
                {
                    if (!dependencyMap.TryGetValue(p, out var kids))
                    {
                        kids = new List<int>();
                        dependencyMap[p] = kids;
                    }
                    kids.Add(i);
                }
            }
        }

        private void CascadeChildren(int parentIndex)
        {
            UpdateDisplay();
            
            if (!dependencyMap.TryGetValue(parentIndex, out var kids)) return;

            foreach (int childIndex in kids)
            {
                MenuGroup menuGroup = menuGroups[childIndex];
                menuGroup.LoadOptions(menuGroups[parentIndex].Text);
                labels[childIndex].text = menuGroup.Text;
            }
        }

        private void OnSelectionUp() {
            IncrementMenuIndex(-1);
        }

        private void OnSelectionDown() {
            IncrementMenuIndex(1);
        }

        private void OnSelectionLeft() {
            menuGroups[menuGroupIndex].IncrementItemIndex(-1);
            CascadeChildren(menuGroupIndex);
        }

        private void OnSelectionRight() {
            menuGroups[menuGroupIndex].IncrementItemIndex(1);
            CascadeChildren(menuGroupIndex);
        }

        private void UpdateDisplay() {
            if (menuGroups.Count > 0) {
                labels[menuGroupIndex].text = menuGroups[menuGroupIndex].Text;

                string ussMenuSelected = "menu-selected";
                foreach (Label label in labels) {
                    label.RemoveFromClassList(ussMenuSelected);
                }
                labels[menuGroupIndex].AddToClassList(ussMenuSelected);
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

            UpdateDisplay();
        }
    }
}
