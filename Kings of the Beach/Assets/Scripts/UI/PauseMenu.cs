using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Menus
{
    public class PauseMenu : MenuNavigation
    {
        [SerializeField] private GameEvent pauseEvent;

        private string hidden = "hidden";

        private VisualElement ui;
        private VisualElement panel;
        private Button resumeButton;
        private Button settingsButton;
        private Button quitButton;

        private MenuButton resumeMenuButton;
        private MenuButton settingsMenuButton;
        private MenuButton quitMenuButton;

        private void Awake() {
            ui = GetComponent<UIDocument>().rootVisualElement;
            panel = ui.Q<VisualElement>("Panel");
            resumeButton = ui.Q<Button>("ResumeButton");
            settingsButton = ui.Q<Button>("SettingsButton");
            quitButton = ui.Q<Button>("QuitButton");

            resumeMenuButton = new MenuButton(resumeButton);
            settingsMenuButton = new MenuButton(settingsButton);
            quitMenuButton = new MenuButton(quitButton);
            resumeMenuButton.SetNavigationUp(quitMenuButton);
            resumeMenuButton.SetNavigationDown(settingsMenuButton);
            settingsMenuButton.SetNavigationUp(resumeMenuButton);
            settingsMenuButton.SetNavigationDown(quitMenuButton);
            quitMenuButton.SetNavigationUp(settingsMenuButton);
            quitMenuButton.SetNavigationDown(resumeMenuButton);

            SetDefault(resumeMenuButton);
        }

        protected override void OnEnable() {
            base.OnEnable();

            resumeMenuButton.ButtonPressed += OnResumeButtonPressed;
            settingsMenuButton.ButtonPressed += OnSettingsButtonPressed;
            quitMenuButton.ButtonPressed += OnQuitButtonPressed;
        }

        protected override void OnDisable() {
            base.OnDisable();

            resumeMenuButton.ButtonPressed -= OnResumeButtonPressed;
            settingsMenuButton.ButtonPressed -= OnSettingsButtonPressed;
            quitMenuButton.ButtonPressed -= OnQuitButtonPressed;
        }

        public void Hide(bool hide) {
            if (hide) {
                panel.AddToClassList(hidden);
            } else {
                panel.RemoveFromClassList(hidden);
            }
        }

        private void OnResumeButtonPressed() {
            pauseEvent.Raise();
        }

        private void OnSettingsButtonPressed() {
            Debug.Log("Settings Menu will load here");
        }

        private void OnQuitButtonPressed() {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}
