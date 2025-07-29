// MenuController.cs - Main menu controller using UI Toolkit
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

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
        private List<MenuItemElement> menuItems = new List<MenuItemElement>();
        private int currentItemIndex = 0;
        private MenuItemElement currentItem;

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
            FindMenuItems();

            if (menuItems.Count > 0)
            {
                SetSelectedItem(0);
            }
        }

        private void FindMenuItems()
        {
            menuItems.Clear();
            
            // Find all menu items in the UI hierarchy
            var allElements = rootElement.Query<VisualElement>().Where(e => e is MenuItemElement).ToList();
            
            foreach (var element in allElements)
            {
                if (element is MenuItemElement menuItem)
                {
                    menuItems.Add(menuItem);
                    menuItem.Initialize(this);
                }
            }

            if (debugMode)
            {
                Debug.Log($"Found {menuItems.Count} menu items");
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
                Debug.Log($"Selected menu item: {currentItem.name}");
            }
        }

        // Public API
        public void SelectItem(int index) => SetSelectedItem(index);
        public void SelectItem(MenuItemElement item)
        {
            int index = menuItems.IndexOf(item);
            if (index >= 0) SetSelectedItem(index);
        }

        public void RegisterMenuItem(MenuItemElement item)
        {
            if (!menuItems.Contains(item))
            {
                menuItems.Add(item);
                item.Initialize(this);
            }
        }

        public VisualElement RootElement => rootElement;
    }
}
