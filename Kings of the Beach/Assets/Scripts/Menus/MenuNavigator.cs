using UnityEngine;
using TMPro;
using UnityEngine.UI; // For working with UI elements

namespace KotB.Menus {

    public class MenuNavigator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] menuOptions;
        
        [Header("Color Options")]
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color defaultColor = Color.white;

        [Header("Input Reader")]
        [SerializeField] private InputReader inputReader;

        // Tracks the currently selected menu index
        private int currentSelectedIndex = 0;

        void Start()
        {
            inputReader.EnableMenuInput();
            
            // Initialize by highlighting the first option
            UpdateSelectedOption();
        }

        private void OnEnable() {
            inputReader.selectionUpEvent += OnSelectionUp;
            inputReader.selectionDownEvent += OnSelectionDown;
            inputReader.selectionLeftEvent += OnSelectionLeft;
            inputReader.selectionRightEvent += OnSelectionRight;
            inputReader.selectEvent += ConfirmSelection;
        }

        private void OnDisable() {
            inputReader.selectionUpEvent -= OnSelectionUp;
            inputReader.selectionDownEvent -= OnSelectionDown;
            inputReader.selectionLeftEvent -= OnSelectionLeft;
            inputReader.selectionRightEvent -= OnSelectionRight;
            inputReader.selectEvent -= ConfirmSelection;
        }

        private void OnSelectionUp() {
            MoveSelection(-1);
        }

        private void OnSelectionDown() {
            MoveSelection(1);
        }

        private void OnSelectionLeft() {
            MoveSelection(-1);
        }

        private void OnSelectionRight() {
            MoveSelection(1);
        }

        private void MoveSelection(int direction)
        {
            // Calculate new index, wrapping around the list
            currentSelectedIndex += direction;
            
            // Ensure index stays within bounds using modulo
            currentSelectedIndex = (currentSelectedIndex + menuOptions.Length) % menuOptions.Length;

            // Update visual selection
            UpdateSelectedOption();
        }

        private void UpdateSelectedOption()
        {
            // Reset all options to default
            for (int i = 0; i < menuOptions.Length; i++)
            {
                menuOptions[i].color = defaultColor;
            }

            // Highlight current selection
            menuOptions[currentSelectedIndex].color = selectedColor;
        }

        private void ConfirmSelection()
        {
            // Trigger action based on selected menu item
            switch(currentSelectedIndex)
            {
                case 0:
                    Debug.Log("Start Game Selected");
                    // Add your start game logic
                    break;
                case 1:
                    Debug.Log("Settings Selected");
                    // Add settings menu logic
                    break;
                case 2:
                    Debug.Log("Quit Game Selected");
                    Application.Quit();
                    break;
            }
        }
    }
}
