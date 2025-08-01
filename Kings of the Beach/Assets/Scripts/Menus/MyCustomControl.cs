using UnityEngine;
using UnityEngine.UIElements;

public class MyCustomControl : VisualElement
{
    public new class UxmlFactory : UxmlFactory<MyCustomControl, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        // Add custom attributes if needed here
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            (ve as MyCustomControl)?.Build();
        }
    }

    public MyCustomControl()
    {
        Build();
    }

    private void Build()
    {
        var label = new Label("Hello from custom control!");
        label.style.color = Color.white;
        Add(label);
    }
}
