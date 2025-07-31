// MenuItemComponent.cs - MonoBehaviour for setting up menu items in Inspector
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace KotB.Menus
{
    public class MenuItemComponent : MonoBehaviour
    {
        [Header("Menu Item Settings")]
        [SerializeField] private MenuItemType itemType = MenuItemType.SimpleButton;
        [SerializeField] private string elementName;
        
        [Header("Simple Button")]
        [SerializeField] private UnityEvent onSelectAction;
        
        [Header("Scene Button")]
        [SerializeField] private string sceneName;
        [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Single;
        
        [Header("Scrollable Option")]
        [SerializeField] private string[] options = { "Option 1", "Option 2", "Option 3" };
        [SerializeField] private int defaultIndex = 0;
        [SerializeField] private bool cycleOptions = true;
        
        [Header("ScriptableObject Option")]
        [SerializeField] private ScriptableObject targetObject;
        [SerializeField] private string fieldName;

        private VisualElement targetElement;
        private IMenuItemHandler handler;

        private void Start()
        {
            SetupMenuItemData();
        }

        private void SetupMenuItemData()
        {
            var uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null) return;

            targetElement = string.IsNullOrEmpty(elementName) 
                ? uiDocument.rootVisualElement.Q<VisualElement>(className: "menu-item")
                : uiDocument.rootVisualElement.Q<VisualElement>(elementName);

            if (targetElement == null) return;

            // Add appropriate class based on type
            switch (itemType)
            {
                case MenuItemType.SceneButton:
                    targetElement.AddToClassList("scene-button");
                    break;
                case MenuItemType.ScrollableOption:
                    targetElement.AddToClassList("scrollable-option");
                    break;
                case MenuItemType.ScriptableObjectOption:
                    targetElement.AddToClassList("scriptable-object-option");
                    targetElement.AddToClassList("scrollable-option");
                    break;
            }
        }

        // Configuration getters for handlers
        public VisualElement GetTargetElement() => targetElement;

        public SimpleButtonConfig GetSimpleButtonConfig()
        {
            return new SimpleButtonConfig
            {
                onSelectAction = onSelectAction
            };
        }

        public SceneConfig GetSceneConfig()
        {
            return new SceneConfig
            {
                sceneName = sceneName,
                loadMode = loadMode
            };
        }

        public ScrollableConfig GetScrollableConfig()
        {
            return new ScrollableConfig
            {
                options = options,
                defaultIndex = defaultIndex,
                cycleOptions = cycleOptions
            };
        }

        public ScriptableObjectConfig GetScriptableObjectConfig()
        {
            return new ScriptableObjectConfig
            {
                targetObject = targetObject,
                fieldName = fieldName,
                options = options,
                defaultIndex = defaultIndex,
                cycleOptions = cycleOptions
            };
        }

        // Configuration structures
        [System.Serializable]
        public struct SimpleButtonConfig
        {
            public UnityEvent onSelectAction;
        }

        [System.Serializable]
        public struct SceneConfig
        {
            public string sceneName;
            public LoadSceneMode loadMode;
        }

        [System.Serializable]
        public struct ScrollableConfig
        {
            public string[] options;
            public int defaultIndex;
            public bool cycleOptions;
        }

        [System.Serializable]
        public struct ScriptableObjectConfig
        {
            public ScriptableObject targetObject;
            public string fieldName;
            public string[] options;
            public int defaultIndex;
            public bool cycleOptions;
        }

        public enum MenuItemType
        {
            SimpleButton,
            SceneButton,
            ScrollableOption,
            ScriptableObjectOption
        }

        // Public method to trigger the action (for simple buttons)
        public void TriggerAction()
        {
            if (itemType == MenuItemType.SimpleButton)
            {
                onSelectAction?.Invoke();
            }
        }
    }
}
