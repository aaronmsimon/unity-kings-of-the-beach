// MenuItemRegistry.cs - Helper to manage menu item components
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace KotB.Menus
{
    public static class MenuItemRegistry
    {
        private static Dictionary<VisualElement, MenuItemComponent> elementToComponentMap = new Dictionary<VisualElement, MenuItemComponent>();

        public static void RegisterMenuItem(VisualElement element, MenuItemComponent component)
        {
            elementToComponentMap[element] = component;
        }

        public static void UnregisterMenuItem(VisualElement element)
        {
            elementToComponentMap.Remove(element);
        }

        public static MenuItemComponent GetComponent(VisualElement element)
        {
            elementToComponentMap.TryGetValue(element, out var component);
            return component;
        }

        public static void Clear()
        {
            elementToComponentMap.Clear();
        }
    }
}
