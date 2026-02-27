using UnityEngine;
using UnityEngine.UIElements;

namespace KotB.Menus.Alt
{
    public abstract class MenuController : MonoBehaviour
    {
        [SerializeField] protected InputReader inputReader;
        [SerializeField] protected UIDocument uiDocument;

        protected virtual void OnEnable() {
            inputReader.EnableMenuInput();

            inputReader.selectionUpEvent += OnSelectionUp;
            inputReader.selectionDownEvent += OnSelectionDown;
            inputReader.selectionLeftEvent += OnSelectionLeft;
            inputReader.selectionRightEvent += OnSelectionRight;
            inputReader.startEvent += OnStart;
        }

        protected virtual void OnDisable() {
            inputReader.selectionUpEvent -= OnSelectionUp;
            inputReader.selectionDownEvent -= OnSelectionDown;
            inputReader.selectionLeftEvent -= OnSelectionLeft;
            inputReader.selectionRightEvent -= OnSelectionRight;
            inputReader.startEvent -= OnStart;
        }

        protected virtual void OnSelectionUp() {}

        protected virtual void OnSelectionDown() {}

        protected virtual void OnSelectionLeft() {}

        protected virtual void OnSelectionRight() {}

        protected virtual void OnStart() {}
    }
}
