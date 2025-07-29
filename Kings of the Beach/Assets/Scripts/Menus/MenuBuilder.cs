// MenuBuilder.cs - Helper class for building menus programmatically
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System.Collections.Generic;

namespace KotB.Menus
{
    public static class MenuBuilder
    {
        public static SimpleMenuButton CreateButton(string text, UnityAction action = null)
        {
            var button = new SimpleMenuButton();
            button.SetText(text);
            
            if (action != null)
            {
                var unityEvent = new UnityEvent();
                unityEvent.AddListener(action);
                button.SetAction(unityEvent);
            }
            
            return button;
        }

        public static SceneMenuButton CreateSceneButton(string text, string sceneName)
        {
            return new SceneMenuButton(text, sceneName);
        }

        public static ScrollableMenuOption CreateScrollableOption(string title, List<string> options, int defaultIndex = 0)
        {
            var option = new ScrollableMenuOption(title, options);
            option.SetSelectedIndex(defaultIndex);
            return option;
        }

        public static ScriptableObjectMenuOption CreateScriptableObjectOption(string title, List<string> options, ScriptableObject target, string fieldName)
        {
            return new ScriptableObjectMenuOption(title, options, target, fieldName);
        }

        public static VisualElement CreateMenuContainer(string className = "menu-container")
        {
            var container = new VisualElement();
            container.AddToClassList(className);
            return container;
        }

        public static VisualElement CreateVerticalMenu(params MenuItemElement[] items)
        {
            var container = CreateMenuContainer("vertical-menu");
            
            foreach (var item in items)
            {
                container.Add(item);
            }
            
            return container;
        }

        public static VisualElement CreateHorizontalMenu(params MenuItemElement[] items)
        {
            var container = CreateMenuContainer("horizontal-menu");
            
            foreach (var item in items)
            {
                container.Add(item);
            }
            
            return container;
        }
    }
}
