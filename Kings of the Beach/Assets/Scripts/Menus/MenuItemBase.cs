using UnityEngine;
using TMPro;

namespace KotB.Menus
{
    public abstract class MenuItemBase : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] protected Color normalColor = Color.white;
        [SerializeField] protected Color selectedColor = Color.yellow;
        [SerializeField] protected TMP_Text displayText;

        protected MenuSystem menuSystem;
        protected bool isSelected = false;

        public virtual void Initialize(MenuSystem system)
        {
            menuSystem = system;
            
            if (displayText == null)
                displayText = GetComponent<TMP_Text>();
                
            UpdateVisuals();
        }

        public virtual void SetSelected(bool selected)
        {
            isSelected = selected;
            UpdateVisuals();
            
            if (selected)
            {
                OnSelected();
            }
            else
            {
                OnDeselected();
            }
        }

        protected virtual void UpdateVisuals()
        {
            if (displayText != null)
            {
                displayText.color = isSelected ? selectedColor : normalColor;
            }
        }

        // Virtual methods for navigation - return true if the item handled the input
        public virtual bool HandleVerticalNavigation(int direction) { return false; }
        public virtual bool HandleHorizontalNavigation(int direction) { return false; }

        // Virtual methods for selection events
        public virtual void OnSelect() { }
        protected virtual void OnSelected() { }
        protected virtual void OnDeselected() { }

        // Properties
        public bool IsSelected => isSelected;
        public MenuSystem MenuSystem => menuSystem;
    }
}
