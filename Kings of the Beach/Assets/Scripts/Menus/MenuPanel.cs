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

        private List<IMenuDisplayable> _values = new();
        private int _currentIndex = 0;
        private bool _isActive = false;

        // UI Elements
        private Label _displayLabel;
        private VisualElement _iconDisplay;

        public IMenuDisplayable CurrentValue => 
            _values.Count > 0 ? _values[_currentIndex] : null;

        public MenuPanel()
        {
            _displayLabel = new Label();
            _iconDisplay = new VisualElement();
            Add(_iconDisplay);
            Add(_displayLabel);
        }

        public void Populate(List<IMenuDisplayable> values, int defaultIndex = 0)
        {
            _values = values;
            _currentIndex = Mathf.Clamp(defaultIndex, 0, values.Count - 1);
            RefreshDisplay();
        }

        public void ResetToDefault(int defaultIndex = 0)
        {
            _currentIndex = Mathf.Clamp(defaultIndex, 0, _values.Count - 1);
            RefreshDisplay();
        }

        public void Activate()
        {
            _isActive = true;
            UpdateVisualState();
        }

        public void Deactivate()
        {
            _isActive = false;
            UpdateVisualState();
        }

        public bool HasValues => _values.Count > 0;

        public void HandleHorizontal(int direction) // -1 left, +1 right
        {
            if (!_isActive || _values.Count == 0) return;

            _currentIndex = (_currentIndex + direction + _values.Count) % _values.Count;
            RefreshDisplay();
            OnValueChanged?.Invoke(this, CurrentValue);
        }

        private void RefreshDisplay()
        {
            if (_values.Count == 0) return;

            var current = _values[_currentIndex];
            _displayLabel.text = current.DisplayName;

            // Icon hookup — USS background-image or sprite renderer, your call later
            // _iconDisplay.style.backgroundImage = ...
        }

        private void UpdateVisualState()
        {
            EnableInClassList("panel-active", _isActive);
            EnableInClassList("panel-inactive", !_isActive);
        }
    }
}
