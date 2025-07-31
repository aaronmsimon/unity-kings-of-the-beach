// SimpleButtonHandler.cs - Handler for simple buttons
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace KotB.Menus
{
    public class SimpleButtonHandler : BaseMenuItemHandler
    {
        private MenuItemComponent menuItemComponent;

        protected override void OnInitialize()
        {
            // Get configuration from MenuItemComponent
            menuItemComponent = FindMenuItemComponent();
        }

        public override void OnSelect()
        {
            if (menuItemComponent != null)
            {
                menuItemComponent.TriggerAction();
            }
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
    }
}
