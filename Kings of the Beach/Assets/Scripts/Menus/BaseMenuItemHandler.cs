// BaseMenuItemHandler.cs - Base implementation for menu item handlers
using UnityEngine.UIElements;

namespace KotB.Menus
{
    public abstract class BaseMenuItemHandler : IMenuItemHandler
    {
        protected MenuController menuController;
        protected VisualElement element;
        protected bool isSelected = false;

        // USS class names
        protected const string SelectedClassName = "menu-item--selected";
        protected const string HoverClassName = "menu-item--hover";

        public virtual void Initialize(MenuController controller, VisualElement element)
        {
            this.menuController = controller;
            this.element = element;

            // Register mouse events
            element.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            element.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
            element.RegisterCallback<ClickEvent>(OnClick);

            OnInitialize();
        }

        public virtual void SetSelected(bool selected)
        {
            if (isSelected == selected) return;

            isSelected = selected;

            if (selected)
            {
                element.AddToClassList(SelectedClassName);
                OnSelected();
            }
            else
            {
                element.RemoveFromClassList(SelectedClassName);
                OnDeselected();
            }
        }

        public virtual bool HandleVerticalNavigation(int direction) => false;
        public virtual bool HandleHorizontalNavigation(int direction) => false;
        public abstract void OnSelect();

        protected virtual void OnInitialize() { }
        protected virtual void OnSelected() { }
        protected virtual void OnDeselected() { }

        private void OnMouseEnter(MouseEnterEvent evt)
        {
            element.AddToClassList(HoverClassName);
            menuController?.SelectItem(menuController.RootElement.Query<VisualElement>(className: "menu-item").ToList().IndexOf(element));
        }

        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            element.RemoveFromClassList(HoverClassName);
        }

        private void OnClick(ClickEvent evt)
        {
            OnSelect();
        }

        public VisualElement Element => element;
    }
}
