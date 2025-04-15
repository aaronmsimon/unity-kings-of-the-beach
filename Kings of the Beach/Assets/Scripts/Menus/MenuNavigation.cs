using UnityEngine;

namespace KotB.Menus
{
    public abstract class MenuNavigation : MonoBehaviour
    {
        [SerializeField] protected InputReader inputReader;
        
        private MenuButton currentButton;

        protected virtual void Start() {
            if (currentButton == null) {
                Debug.LogWarning($"No default menu button set on {name}");
                return;
            }

            currentButton.ButtonSelected();
        }

        protected virtual void OnEnable() {
            inputReader.selectionUpEvent += OnSelectionUp;
            inputReader.selectionDownEvent += OnSelectionDown;
            inputReader.selectionLeftEvent += OnSelectionLeft;
            inputReader.selectionRightEvent += OnSelectionRight;
            inputReader.startEvent += OnStart;
            inputReader.selectEvent += OnSelect;
        }

        protected virtual void OnDisable() {
            inputReader.selectionUpEvent -= OnSelectionUp;
            inputReader.selectionDownEvent -= OnSelectionDown;
            inputReader.selectionLeftEvent -= OnSelectionLeft;
            inputReader.selectionRightEvent -= OnSelectionRight;
            inputReader.startEvent -= OnStart;
            inputReader.selectEvent -= OnSelect;
        }

        protected void SetDefault(MenuButton menuButton) {
            currentButton = menuButton;
        }

        private void OnSelectionUp() {
            MenuButton menuButton = currentButton?.NavigateUp();
            if (menuButton != null) {
                currentButton.ButtonDeselected();
                currentButton = menuButton;
                currentButton.ButtonSelected();
            }
        }

        private void OnSelectionDown() {
            MenuButton menuButton = currentButton?.NavigateDown();
            if (menuButton != null) {
                currentButton.ButtonDeselected();
                currentButton = menuButton;
                currentButton.ButtonSelected();
            }
        }

        private void OnSelectionLeft() {
            MenuButton menuButton = currentButton?.NavigateLeft();
            if (menuButton != null) {
                currentButton.ButtonDeselected();
                currentButton = menuButton;
                currentButton.ButtonSelected();
            }
        }

        private void OnSelectionRight() {
            MenuButton menuButton = currentButton?.NavigateRight();
            if (menuButton != null) {
                currentButton.ButtonDeselected();
                currentButton = menuButton;
                currentButton.ButtonSelected();
            }
        }

        public virtual void OnStart() { }

        private void OnSelect() {
            currentButton.ButtonPressedEvent();
        }
    }
}
