using UnityEngine;
using KotB.Menus;
using KotB.Menus.Alt;

namespace KotB.Items
{
    [CreateAssetMenu(fileName = "MaterialSO", menuName = "Game/Material SO")]
    public class MaterialSO : ScriptableObject, IMenuSelection, IMenuDisplayable
    {
        [SerializeField] private Material mat;

        [Header("Menu Display")]
        [SerializeField] private string displayName;

        // --- PROPERTIES ---
        public Material Mat => mat;
        public string DisplayName => displayName;

        public string GetMenuText()
        {
            return name;
        }

        public string GetMenuKey()
        {
            return GetMenuText();
        }
    }
}
