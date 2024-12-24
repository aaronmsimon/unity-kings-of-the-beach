using UnityEngine;

namespace KotB.Menus {

    public class MenuNavigator : MonoBehaviour
    {
        [SerializeField] private UIMenuSelectable defaultSelection;
        
        [Header("Color Options")]
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color defaultColor = Color.white;

        [Header("Input Reader")]
        [SerializeField] private InputReader inputReader;

        private UIMenuSelectable currentSelection;

        void Start()
        {
            inputReader.EnableMenuInput();
            
            currentSelection = defaultSelection;
            UpdateSelectedOption(defaultSelection);
        }

        private void OnEnable() {
            inputReader.selectionUpEvent += OnSelectionUp;
            inputReader.selectionDownEvent += OnSelectionDown;
            inputReader.selectionLeftEvent += OnSelectionLeft;
            inputReader.selectionRightEvent += OnSelectionRight;
            inputReader.selectEvent += OnSelected;
        }

        private void OnDisable() {
            inputReader.selectionUpEvent -= OnSelectionUp;
            inputReader.selectionDownEvent -= OnSelectionDown;
            inputReader.selectionLeftEvent -= OnSelectionLeft;
            inputReader.selectionRightEvent -= OnSelectionRight;
            inputReader.selectEvent -= OnSelected;
        }

        private void GoToNextUISelectable() {
            UpdateSelectedOption(currentSelection.NextUISelectable);
        }

        private void GoToPrevUISelectable() {
            UpdateSelectedOption(currentSelection.PrevUISelectable);
        }

        private void OnSelectionUp() {
            if (currentSelection is UIGroupSelect) {
                GoToPrevUISelectable();
            } else {
                Debug.LogAssertion("Can't go up");
            }
        }

        private void OnSelectionDown() {
            if (currentSelection is UIGroupSelect) {
                GoToNextUISelectable();
            } else {
                Debug.LogAssertion("Can't go down");
            }
        }

        private void OnSelectionLeft() {
            if (currentSelection is UIGroupSelect) {
                UIGroupSelect grp = (UIGroupSelect)currentSelection;
                grp.IncrementItemIndex(-1);
            } else {
                Debug.LogAssertion("Can't go left");
            }
        }

        private void OnSelectionRight() {
            if (currentSelection is UIGroupSelect) {
                UIGroupSelect grp = (UIGroupSelect)currentSelection;
                grp.IncrementItemIndex(1);
            } else {
                Debug.LogAssertion("Can't go right");
            }
        }

        private void UpdateSelectedOption(UIMenuSelectable newSelection)
        {
            currentSelection.MenuText.color = defaultColor;
            currentSelection = newSelection;
            currentSelection.MenuText.color = selectedColor;
        }

        private void OnSelected()
        {
            currentSelection.Selected();
        }
    }
}
