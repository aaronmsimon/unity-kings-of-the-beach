using UnityEngine;
using TMPro;

namespace KotB.Menus
{
    public abstract class UIMenuSelectable : MonoBehaviour
    {
        [Header("Navigation")]
        [SerializeField] private UIMenuSelectable prevUISelectable;
        [SerializeField] private UIMenuSelectable nextUISelectable;

        protected TMP_Text _menuText;

        public virtual void Awake() {
            _menuText = GetComponent<TMP_Text>();
        }

        public virtual void Selected() {
            Debug.Log($"{name} was selected with value {_menuText.text}");
        }

        // --- PROPERTIES ---
        public UIMenuSelectable PrevUISelectable => prevUISelectable;
        public UIMenuSelectable NextUISelectable => nextUISelectable;
        public TMP_Text MenuText { get { return _menuText; } set { _menuText = value; } }
    }
}
