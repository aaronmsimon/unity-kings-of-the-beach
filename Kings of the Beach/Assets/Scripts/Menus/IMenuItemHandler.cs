// IMenuItemHandler.cs - Interface for menu item handlers
using UnityEngine.UIElements;

namespace KotB.Menus
{
    public interface IMenuItemHandler
    {
        void Initialize(MenuController controller, VisualElement element);
        void SetSelected(bool selected);
        bool HandleVerticalNavigation(int direction);
        bool HandleHorizontalNavigation(int direction);
        void OnSelect();
        VisualElement Element { get; }
    }
}
