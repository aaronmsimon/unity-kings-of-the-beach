using UnityEngine;
using UnityEngine.UIElements;

namespace KotB.Menus
{
    public class PopulateMenu : MonoBehaviour
    {
        private void Awake() {
            Label label = new Label("Test Label");

            label.name = "testLabel";
            label.AddToClassList("unity-text-element");
            label.AddToClassList("unity-label");
            label.AddToClassList("menu-list-selection");

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement panel = root.Query<VisualElement>("Panel");
            panel.Add(label);
        }
    }
}
