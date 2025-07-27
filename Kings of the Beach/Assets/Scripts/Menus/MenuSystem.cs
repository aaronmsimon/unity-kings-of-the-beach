using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KotB.Menus
{
    public class MenuSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool cycleThroughItems = true;
        [SerializeField] private bool debugMode = false;
        
        [Header("Menu Items")]
        [SerializeField] private List<MenuItemBase> menuItems = new List<MenuItemBase>();
        
        [Header("Input")]
        [SerializeField] private InputReader inputReader;

        private int currentItemIndex = 0;
        private MenuItemBase currentItem;

        private void Start()
        {
            if (menuItems.Count == 0)
            {
                FindMenuItems();
            }
            
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

        private void FindMenuItems()
        {
            menuItems = GetComponentsInChildren<MenuItemBase>().ToList();
            
            // Sort by hierarchy order
            menuItems.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
        }

        private void InitializeMenu()
        {
            if (menuItems.Count == 0) return;

            // Initialize all items
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i].Initialize(this);
                menuItems[i].SetSelected(false);
            }

            // Select first item
            SetSelectedItem(0);
        }

        private void OnSelectionUp()
        {
            if (currentItem != null && currentItem.HandleVerticalNavigation(-1))
                return; // Item handled the input

            NavigateVertical(-1);
        }

        private void OnSelectionDown()
        {
            if (currentItem != null && currentItem.HandleVerticalNavigation(1))
                return; // Item handled the input

            NavigateVertical(1);
        }

        private void OnSelectionLeft()
        {
            if (currentItem != null && currentItem.HandleHorizontalNavigation(-1))
                return; // Item handled the input

            NavigateVertical(-1); // Default behavior for left
        }

        private void OnSelectionRight()
        {
            if (currentItem != null && currentItem.HandleHorizontalNavigation(1))
                return; // Item handled the input

            NavigateVertical(1); // Default behavior for right
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

            // Deselect current item
            if (currentItem != null)
            {
                currentItem.SetSelected(false);
            }

            // Select new item
            currentItemIndex = index;
            currentItem = menuItems[currentItemIndex];
            currentItem.SetSelected(true);

            if (debugMode)
            {
                Debug.Log($"Selected menu item: {currentItem.name}");
            }
        }

        // Public methods for external control
        public void SelectItem(int index)
        {
            SetSelectedItem(index);
        }

        public void SelectItem(MenuItemBase item)
        {
            int index = menuItems.IndexOf(item);
            if (index >= 0)
            {
                SetSelectedItem(index);
            }
        }

        public void AddMenuItem(MenuItemBase item)
        {
            if (!menuItems.Contains(item))
            {
                menuItems.Add(item);
                item.Initialize(this);
            }
        }

        public void RemoveMenuItem(MenuItemBase item)
        {
            int index = menuItems.IndexOf(item);
            if (index >= 0)
            {
                menuItems.RemoveAt(index);
                if (currentItemIndex >= menuItems.Count)
                {
                    SetSelectedItem(menuItems.Count - 1);
                }
            }
        }
    }
}
