using UnityEngine;
using System.Collections.Generic;

namespace KotB.Menus
{
    public class ScrollableMenuItem : MenuItemBase
    {
        [Header("Scrollable Settings")]
        [SerializeField] private List<string> options = new List<string>();
        [SerializeField] private int defaultOptionIndex = 0;
        [SerializeField] private bool cycleOptions = true;
        [SerializeField] private string displayFormat = "{0}"; // Use {0} for option placeholder

        private int currentOptionIndex = 0;

        public override void Initialize(MenuSystem system)
        {
            base.Initialize(system);
            currentOptionIndex = Mathf.Clamp(defaultOptionIndex, 0, options.Count - 1);
            UpdateDisplay();
        }

        public override bool HandleHorizontalNavigation(int direction)
        {
            if (options.Count <= 1) return false;

            int newIndex = currentOptionIndex + direction;

            if (cycleOptions)
            {
                if (newIndex < 0)
                    newIndex = options.Count - 1;
                else if (newIndex >= options.Count)
                    newIndex = 0;
            }
            else
            {
                newIndex = Mathf.Clamp(newIndex, 0, options.Count - 1);
            }

            if (newIndex != currentOptionIndex)
            {
                currentOptionIndex = newIndex;
                UpdateDisplay();
                OnOptionChanged();
            }

            return true; // We handled the input
        }

        private void UpdateDisplay()
        {
            if (displayText != null && options.Count > 0 && currentOptionIndex < options.Count)
            {
                string optionText = options[currentOptionIndex];
                displayText.text = string.Format(displayFormat, optionText);
            }
        }

        protected virtual void OnOptionChanged()
        {
            // Override in derived classes for custom behavior
        }

        // Public methods
        public void SetOptions(List<string> newOptions)
        {
            options = newOptions;
            currentOptionIndex = Mathf.Clamp(currentOptionIndex, 0, options.Count - 1);
            UpdateDisplay();
        }

        public void SetSelectedOption(int index)
        {
            if (index >= 0 && index < options.Count)
            {
                currentOptionIndex = index;
                UpdateDisplay();
                OnOptionChanged();
            }
        }

        public void SetSelectedOption(string option)
        {
            int index = options.IndexOf(option);
            if (index >= 0)
            {
                SetSelectedOption(index);
            }
        }

        // Properties
        public string CurrentOption => options.Count > 0 && currentOptionIndex < options.Count ? options[currentOptionIndex] : "";
        public int CurrentOptionIndex => currentOptionIndex;
        public List<string> Options => new List<string>(options);
    }
}
