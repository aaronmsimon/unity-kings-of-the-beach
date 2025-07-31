// MenuController.cs - Main menu controller using UI Toolkit
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace KotB.Menus
{
    public class MenuController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool cycleThroughItems = true;
        [SerializeField] private bool debugMode = false;
        
        [Header("Input")]
        [SerializeField] private InputReader inputReader;
        
        [Header("UI Document")]
        [SerializeField] private UIDocument uiDocument;

        private VisualElement rootElement;
        private List<IMenuItemHandler> menuItems = new List<IMenuItemHandler>();
        private int currentItemIndex = 0;
        private IMenuItemHandler currentItem;

        private void Awake()
        {
            if (uiDocument == null)
                uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            InitializeMenu();
        }

        private void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.EnableMenuInput();
                inputReader.selectionUpEvent += OnSelectionUp;
                inputReader.selectionDownEvent += OnSelectionDown;
                inputReader.selectionLeftEvent += OnSelectionLeft;
                inputReader.selectionRightEvent += OnSelectionRight;
                inputReader.selectEvent += OnSelect;
                inputReader.startEvent += OnSelect;
            }
        }

        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.selectionUpEvent -= OnSelectionUp;
                inputReader.selectionDownEvent -= OnSelectionDown;
                inputReader.selectionLeftEvent -= OnSelectionLeft;
                inputReader.selectionRightEvent -= OnSelectionRight;
                inputReader.selectEvent -= OnSelect;
                inputReader.startEvent -= OnSelect;
            }
        }

        private void InitializeMenu()
        {
            if (uiDocument == null) return;

            rootElement = uiDocument.rootVisualElement;
            FindAndSetupMenuItems();

            if (menuItems.Count > 0)
            {
                SetSelectedItem(0);
            }
        }

        private void FindAndSetupMenuItems()
        {
            menuItems.Clear();
            
            // Find all elements with menu-item class
            var menuElements = rootElement.Query<VisualElement>(className: "menu-item").ToList();
            
            foreach (var element in menuElements)
            {
                var handler = CreateHandlerForElement(element);
                if (handler != null)
                {
                    menuItems.Add(handler);
                    handler.Initialize(this, element);
                }
            }

            if (debugMode)
            {
                Debug.Log($"Found {menuItems.Count} menu items");
            }
        }

        private IMenuItemHandler CreateHandlerForElement(VisualElement element)
        {
            // Check data attributes or class names to determine handler type
            if (element.ClassListContains("scrollable-option"))
            {
                return new ScrollableOptionHandler();
            }
            else if (element.ClassListContains("scene-button"))
            {
                return new SceneButtonHandler();
            }
            else if (element.ClassListContains("scriptable-object-option"))
            {
                return new ScriptableObjectOptionHandler();
            }
            else
            {
                return new SimpleButtonHandler();
            }
        }

        private void OnSelectionUp()
        {
            if (currentItem != null && currentItem.HandleVerticalNavigation(-1))
                return;

            NavigateVertical(-1);
        }

        private void OnSelectionDown()
        {
            if (currentItem != null && currentItem.HandleVerticalNavigation(1))
                return;

            NavigateVertical(1);
        }

        private void OnSelectionLeft()
        {
            if (currentItem != null && currentItem.HandleHorizontalNavigation(-1))
                return;

            NavigateVertical(-1);
        }

        private void OnSelectionRight()
        {
            if (currentItem != null && currentItem.HandleHorizontalNavigation(1))
                return;

            NavigateVertical(1);
        }

        private void OnSelect()
        {
            currentItem?.OnSelect();
        }

        private void NavigateVertical(int direction)
        {
            if (menuItems.Count <= 1) return;

            int newIndex = currentItemIndex + direction;

            if (cycleThroughItems)
            {
                if (newIndex < 0)
                    newIndex = menuItems.Count - 1;
                else if (newIndex >= menuItems.Count)
                    newIndex = 0;
            }
            else
            {
                newIndex = Mathf.Clamp(newIndex, 0, menuItems.Count - 1);
            }

            SetSelectedItem(newIndex);
        }

        private void SetSelectedItem(int index)
        {
            if (index < 0 || index >= menuItems.Count) return;

            currentItem?.SetSelected(false);

            currentItemIndex = index;
            currentItem = menuItems[currentItemIndex];
            currentItem.SetSelected(true);

            if (debugMode)
            {
                Debug.Log($"Selected menu item: {currentItem.Element.name}");
            }
        }

        // Public API
        public void SelectItem(int index) => SetSelectedItem(index);
        public VisualElement RootElement => rootElement;
    }
}
