using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace KotB.Menus.Alt
{
    public abstract class MenuController : MonoBehaviour
    {
        [SerializeField] protected InputReader inputReader;
        [SerializeField] protected UIDocument uiDocument;

        protected List<MenuPanel> panels = new();
        protected int activePanelIndex = 0;

        protected virtual void OnEnable() {
            inputReader.EnableMenuInput();

            inputReader.selectionUpEvent += OnSelectionUp;
            inputReader.selectionDownEvent += OnSelectionDown;
            inputReader.selectionLeftEvent += OnSelectionLeft;
            inputReader.selectionRightEvent += OnSelectionRight;
            inputReader.startEvent += OnStart;
        }

        protected virtual void OnDisable() {
            inputReader.selectionUpEvent -= OnSelectionUp;
            inputReader.selectionDownEvent -= OnSelectionDown;
            inputReader.selectionLeftEvent -= OnSelectionLeft;
            inputReader.selectionRightEvent -= OnSelectionRight;
            inputReader.startEvent -= OnStart;
        }

        protected virtual void OnSelectionUp() {
            int next = (activePanelIndex - 1 + panels.Count) % panels.Count;
            SetActivePanel(next);
        }

        protected virtual void OnSelectionDown() {
            int next = (activePanelIndex + 1) % panels.Count;
            SetActivePanel(next);
        }

        protected virtual void OnSelectionLeft() {
            var activePanel = panels[activePanelIndex];
            if (activePanel.HasValues)
                activePanel.HandleHorizontal(-1);
            else
                OnSelectionUp();
        }

        protected virtual void OnSelectionRight() {
            var activePanel = panels[activePanelIndex];
            if (activePanel.HasValues)
                activePanel.HandleHorizontal(1);
            else
                OnSelectionDown();
        }

        protected virtual void OnStart() {}

        protected void Quit() {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        protected void SetActivePanel(int index)
        {
            panels[activePanelIndex].Deactivate();
            activePanelIndex = index;
            panels[activePanelIndex].Activate();
        }
    }
}
