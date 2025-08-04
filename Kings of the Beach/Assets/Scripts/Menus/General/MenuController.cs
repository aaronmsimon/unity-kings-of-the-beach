using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MenuSystem
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private List<MenuOptionsSO> menuOptions;
        [SerializeField] private InputReader inputReader;

        public event Action SelectionChanged;

        private List<Label> menuListSelections;

        private void Awake() {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            menuListSelections = root.Query<Label>(className: "menu-list-selection").ToList();

            foreach (Label menuList in menuListSelections) {
                Debug.Log(menuList.text);
            }
        }

        protected void UpdateDisplay() {
            // if (groupItems.Length > 0) {
            //     _menuText.text = groupItems[groupItemIndex];
            // }
        }

        protected void RaiseSelectionChangedEvent() {
            // SelectionChanged?.Invoke();
        }
    }
}
