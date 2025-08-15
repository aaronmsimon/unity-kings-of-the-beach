using UnityEngine;
using KotB.Menus;

namespace KotB.Items
{
    [CreateAssetMenu(fileName = "MaterialSO", menuName = "Game/Material SO")]
    public class MaterialSO : ScriptableObject, IMenuSelection
    {
        [SerializeField] private Material mat;

        // --- PROPERTIES ---
        public Material Mat => mat;

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
