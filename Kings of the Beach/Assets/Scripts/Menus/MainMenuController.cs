using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KotB.Menus.Alt
{
    public class MainMenuController : MenuController
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private List<MainMenuItemData> menuItems;

        private void Awake() {
            inputReader.EnableMenuInput();
        }

        private void OnEnable() {
            Activate();
            BuildMenu();
        }

        private void OnDisable() {
            Deactivate();
        }

        private void BuildMenu()
        {
            var root = uiDocument.rootVisualElement.Q("root");
            panels.Clear();

            foreach (var item in menuItems)
            {
                var panel = new MenuPanel();
                panel.Populate(new List<IMenuDisplayable> { item });
                root.Add(panel);
                panels.Add(panel);
            }

            SetActivePanel(0);
        }

        protected override void OnStart() {
            MainMenuItemData selected = (MainMenuItemData)panels[activePanelIndex].CurrentValue;
            if (selected == null) return;

            if (string.IsNullOrEmpty(selected.SceneName))
            {
                Quit();
                return;
            }

            SceneManager.LoadScene(selected.SceneName);
        }

        [Serializable]
        private class MainMenuItemData : IMenuDisplayable
        {
            [SerializeField] private string displayName;
            [SerializeField] private string sceneName;

            public string DisplayName => displayName;
            public string SceneName => sceneName;
        }
    }
}
