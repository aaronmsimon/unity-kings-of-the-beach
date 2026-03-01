using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace KotB.Menus.Alt
{
    public class MenuPanel : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<MenuPanel> {}

        public event Action<MenuPanel, IMenuDisplayable> OnValueChanged;

        private List<IMenuDisplayable> values = new();
        private int currentIndex = 0;
        private bool isActive = false;
        private Label displayLabel;

        public MenuPanel() {
            displayLabel = new Label();
            Add(displayLabel);
        }

        public void Populate(List<IMenuDisplayable> values, int defaultIndex = 0) {
            this.values = values;
            currentIndex = Mathf.Clamp(defaultIndex, 0, values.Count - 1);
            RefreshDisplay();
        }

        public void ResetToDefault(int defaultIndex = 0) {
            currentIndex = Mathf.Clamp(defaultIndex, 0, values.Count - 1);
            RefreshDisplay();
        }

        public void Activate() {
            isActive = true;
            UpdateVisualState();
        }

        public void Deactivate() {
            isActive = false;
            UpdateVisualState();
        }

        public void HandleHorizontal(int direction) {
            if (!isActive || values.Count == 0) return;

            currentIndex = (currentIndex + direction + values.Count) % values.Count;
            RefreshDisplay();
            OnValueChanged?.Invoke(this, CurrentValue);
        }

        private void RefreshDisplay() {
            if (values.Count == 0) return;

            var current = values[currentIndex];
            displayLabel.text = current.DisplayName;
        }

        private void UpdateVisualState() {
            EnableInClassList("panel-active", isActive);
            EnableInClassList("panel-inactive", !isActive);
        }

        public IMenuDisplayable CurrentValue => values.Count > 0 ? values[currentIndex] : null;
        public bool IsMulti => values.Count > 1;
    }
}
