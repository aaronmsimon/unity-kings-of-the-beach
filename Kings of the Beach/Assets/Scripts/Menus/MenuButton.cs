using System;
using UnityEngine.UIElements;

namespace KotB.Menus
{
    public class MenuButton
    {
        public event Action ButtonPressed;

        private Button button;
        private MenuButton up;
        private MenuButton down;
        private MenuButton left;
        private MenuButton right;

        public MenuButton(Button button) {
            this.button = button;
        }

        public void SetNavigationUp(MenuButton menuButton) {
            up = menuButton;
        }

        public void SetNavigationDown(MenuButton menuButton) {
            down = menuButton;
        }

        public void SetNavigationLeft(MenuButton menuButton) {
            left = menuButton;
        }

        public void SetNavigationRight(MenuButton menuButton) {
            right = menuButton;
        }

        public MenuButton NavigateUp() {
            return up;
        }

        public MenuButton NavigateDown() {
            return down;
        }

        public MenuButton NavigateLeft() {
            return left;
        }

        public MenuButton NavigateRight() {
            return right;
        }

        public void ButtonPressedEvent() {
            ButtonPressed?.Invoke();
        }

        public void ButtonSelected() {
            button.AddToClassList("selected");
        }

        public void ButtonDeselected() {
            button.RemoveFromClassList("selected");
        }
    }
}
