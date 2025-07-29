// ScrollableMenuOption.cs - Menu item with scrollable options
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

namespace KotB.Menus
{
    public class ScrollableMenuOption : MenuItemElement
    {
        private Label titleLabel;
        private Label valueLabel;
        private List<string> options = new List<string>();
        private int currentIndex = 0;
        private bool cycleOptions = true;

        protected new const string UssClassName = "scrollable-menu-option";

        public event Action<string> OnValueChanged;

        public ScrollableMenuOption() : this("Option", new List<string> { "Value" }) { }

        public ScrollableMenuOption(string title, List<string> optionsList)
        {
            AddToClassList(UssClassName);

            // Create container for horizontal layout
            var container = new VisualElement();
            container.AddToClassList("scrollable-menu-option__container");
            Add(container);

            // Title label
            titleLabel = new Label(title);
            titleLabel.AddToClassList("scrollable-menu-option__title");
            container.Add(titleLabel);

            // Value label
            valueLabel = new Label();
            valueLabel.AddToClassList("scrollable-menu-option__value");
            container.Add(valueLabel);

            SetOptions(optionsList);
        }

        public override bool HandleHorizontalNavigation(int direction)
        {
            if (options.Count <= 1) return false;

            int newIndex = currentIndex + direction;

            if (cycleOptions)
            {
                if (newIndex < 0)
                    newIndex = options.Count - 1;
                else if (newIndex >= options.Count)
                    newIndex = 0;
            }
            else
            {
                newIndex = UnityEngine.Mathf.Clamp(newIndex, 0, options.Count - 1);
            }

            if (newIndex != currentIndex)
            {
                currentIndex = newIndex;
                UpdateValueDisplay();
                OnValueChanged?.Invoke(CurrentValue);
            }

            return true; // We handled the input
        }

        private void UpdateValueDisplay()
        {
            if (valueLabel != null && options.Count > 0 && currentIndex < options.Count)
            {
                valueLabel.text = options[currentIndex];
            }
        }

        public void SetOptions(List<string> newOptions)
        {
            options = new List<string>(newOptions);
            currentIndex = UnityEngine.Mathf.Clamp(currentIndex, 0, options.Count - 1);
            UpdateValueDisplay();
        }

        public void SetSelectedIndex(int index)
        {
            if (index >= 0 && index < options.Count)
            {
                currentIndex = index;
                UpdateValueDisplay();
                OnValueChanged?.Invoke(CurrentValue);
            }
        }

        public void SetSelectedValue(string value)
        {
            int index = options.IndexOf(value);
            if (index >= 0)
                SetSelectedIndex(index);
        }

        public void SetTitle(string title)
        {
            if (titleLabel != null)
                titleLabel.text = title;
        }

        public void SetCycling(bool cycle)
        {
            cycleOptions = cycle;
        }

        public string CurrentValue => options.Count > 0 && currentIndex < options.Count ? options[currentIndex] : "";
        public int CurrentIndex => currentIndex;
        public List<string> Options => new List<string>(options);
        public string Title => titleLabel?.text ?? "";
    }
}
