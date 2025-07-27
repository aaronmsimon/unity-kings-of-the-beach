using UnityEngine;
using UnityEngine.Events;

namespace KotB.Menus
{
    public class SimpleMenuItem : MenuItemBase
    {
        [Header("Actions")]
        [SerializeField] private UnityEvent onSelectAction;

        public override void OnSelect()
        {
            onSelectAction?.Invoke();
        }
    }
}
