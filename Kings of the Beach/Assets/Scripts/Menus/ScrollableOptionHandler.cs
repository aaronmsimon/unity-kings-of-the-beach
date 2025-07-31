// ScrollableOptionHandler.cs - Handler for scrollable options
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

namespace KotB.Menus
{
    public class ScrollableOptionHandler : BaseMenuItemHandler
    {
        private Label valueLabel;
        private List<string> options = new List<string>();
        private int currentIndex = 0;
        private bool cycleOptions = true;

        protected override void OnInitialize()
        {
            // Find the value label (should have name "option-value")
            valueLabel = element.Q<Label>("option-value");
            
            // Get configuration from MenuItemComponent
            var menuItemComponent = FindMenuItemComponent();
            if (menuItemComponent != null)
            {
                var config = menuItemComponent.GetScrollableConfig();
                options = new List<string>(config.options);
                currentIndex = Mathf.Clamp(config.defaultIndex, 0, options.Count - 1);
                cycleOptions = config.cycleOptions;
            }
            else
            {
                // Default options if no component found
                options = new List<string> { "Option 1", "Option 2", "Option 3" };
                currentIndex = 0;
            }

            UpdateValueDisplay();
        }

        private MenuItemComponent FindMenuItemComponent()
        {
            // Try to find MenuItemComponent in the scene that references this element
            var menuItemComponents = Object.FindObjectsOfType<MenuItemComponent>();
            foreach (var component in menuItemComponents)
            {
                if (component.GetTargetElement() == element)
                {
                    return component;
                }
            }
            return null;
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
                newIndex = Mathf.Clamp(newIndex, 0, options.Count - 1);
            }

            if (newIndex != currentIndex)
            {
                currentIndex = newIndex;
                UpdateValueDisplay();
                OnValueChanged();
            }

            return true;
        }

        public override void OnSelect()
        {
            // Could cycle to next option on select
            HandleHorizontalNavigation(1);
        }

        private void UpdateValueDisplay()
        {
            if (valueLabel != null && options.Count > 0 && currentIndex < options.Count)
            {
                valueLabel.text = options[currentIndex];
            }
        }

        protected virtual void OnValueChanged()
        {
            // Override in derived classes
        }

        public string CurrentValue => options.Count > 0 && currentIndex < options.Count ? options[currentIndex] : "";
        public int CurrentIndex => currentIndex;
    }
}
