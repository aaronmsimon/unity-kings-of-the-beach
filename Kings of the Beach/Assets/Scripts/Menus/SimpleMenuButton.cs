// SimpleMenuButton.cs - Basic menu button
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace KotB.Menus
{
    public class SimpleMenuButton : MenuItemElement
    {
        private Label label;
        private UnityEvent onSelectAction;

        protected new const string UssClassName = "simple-menu-button";

        public SimpleMenuButton() : this("Menu Item", null) { }

        public SimpleMenuButton(string text, UnityEvent selectAction)
        {
            AddToClassList(UssClassName);
            
            label = new Label(text);
            label.AddToClassList("simple-menu-button__label");
            Add(label);

            onSelectAction = selectAction;
        }

        public override void OnSelect()
        {
            onSelectAction?.Invoke();
        }

        public void SetText(string text)
        {
            if (label != null)
                label.text = text;
        }

        public void SetAction(UnityEvent action)
        {
            onSelectAction = action;
        }

        public string Text => label?.text ?? "";
    }
}
