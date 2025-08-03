using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MenuSystem
{
    public class MenuController : MonoBehaviour
    {
        private List<Label> menuListSelections;

        private void Awake() {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            menuListSelections = root.Query<Label>(className: "menu-list-selection").ToList();

            foreach (Label menuList in menuListSelections) {
                Debug.Log(menuList.text);
            }
        }
    }
}
