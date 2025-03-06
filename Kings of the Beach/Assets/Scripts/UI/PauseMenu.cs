using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using RoboRyanTron.Unite2017.Events;

using System.Collections.Generic;

namespace KotB.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameEvent pauseEvent;

        private string hidden = "hidden";

        private VisualElement ui;
        private VisualElement panel;
        private Button resumeButton;
        private Button settingsButton;
        private Button quitButton;

        private void Awake() {
            ui = GetComponent<UIDocument>().rootVisualElement;
            panel = ui.Q<VisualElement>("Panel");
            resumeButton = ui.Q<Button>("ResumeButton");
            settingsButton = ui.Q<Button>("SettingsButton");
            quitButton = ui.Q<Button>("QuitButton");
        }

        private void OnEnable() {
            resumeButton.clicked += OnResumeButtonClicked;
            settingsButton.clicked += OnSettingsButtonClicked;
            quitButton.clicked += OnQuitButtonClicked;
        }

        private void OnDisable() {
            resumeButton.clicked -= OnResumeButtonClicked;
            settingsButton.clicked -= OnSettingsButtonClicked;
            quitButton.clicked -= OnQuitButtonClicked;
        }

        public void Hide(bool hide) {
            if (hide) {
                panel.AddToClassList(hidden);
            } else {
                panel.RemoveFromClassList(hidden);
            }
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
