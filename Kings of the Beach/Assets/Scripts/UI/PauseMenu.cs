using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameEvent pauseEvent;

        private VisualElement ui;
        private Button resumeButton;
        private Button settingsButton;
        private Button quitButton;

        private void OnEnable() {
            ui = GetComponent<UIDocument>().rootVisualElement;

            resumeButton = ui.Q<Button>("ResumeButton");
            resumeButton.clicked += OnResumeButtonClicked;

            settingsButton = ui.Q<Button>("SettingsButton");
            settingsButton.clicked += OnSettingsButtonClicked;
            
            quitButton = ui.Q<Button>("QuitButton");
            quitButton.clicked += OnQuitButtonClicked;
        }

        private void OnDisable() {
            resumeButton.clicked -= OnResumeButtonClicked;
            settingsButton.clicked -= OnSettingsButtonClicked;
            quitButton.clicked -= OnQuitButtonClicked;
        }

        private void OnResumeButtonClicked() {
            pauseEvent.Raise();
        }

        private void OnSettingsButtonClicked() {
            Debug.Log("Settings Menu will load here");
        }

        private void OnQuitButtonClicked() {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}
