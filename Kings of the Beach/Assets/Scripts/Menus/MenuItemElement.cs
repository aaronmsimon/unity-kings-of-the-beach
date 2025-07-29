// MenuItemElement.cs - Base class for all menu items
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace KotB.Menus
{
    public abstract class MenuItemElement : VisualElement
    {
        protected MenuController menuController;
        protected bool isSelected = false;

        // USS class names for styling
        protected const string UssClassName = "menu-item";
        protected const string UssSelectedClassName = "menu-item--selected";
        protected const string UssHoverClassName = "menu-item--hover";

        public MenuItemElement()
        {
            AddToClassList(UssClassName);
            
            // Register for mouse events
            RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
            RegisterCallback<ClickEvent>(OnClick);
        }

        public virtual void Initialize(MenuController controller)
        {
            menuController = controller;
        }

        public virtual void SetSelected(bool selected)
        {
            if (isSelected == selected) return;

            isSelected = selected;

            if (selected)
            {
                AddToClassList(UssSelectedClassName);
                OnSelected();
            }
            else
            {
                RemoveFromClassList(UssSelectedClassName);
                OnDeselected();
            }
        }

        // Navigation handlers - return true if handled
        public virtual bool HandleVerticalNavigation(int direction) => false;
        public virtual bool HandleHorizontalNavigation(int direction) => false;

        // Selection events
        public virtual void OnSelect() { }
        protected virtual void OnSelected() { }
        protected virtual void OnDeselected() { }

        // Mouse events
        private void OnMouseEnter(MouseEnterEvent evt)
        {
            AddToClassList(UssHoverClassName);
            menuController?.SelectItem(this);
        }

        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            RemoveFromClassList(UssHoverClassName);
        }

        private void OnClick(ClickEvent evt)
        {
            OnSelect();
        }

        public bool IsSelected => isSelected;
        public MenuController MenuController => menuController;
    }
}
