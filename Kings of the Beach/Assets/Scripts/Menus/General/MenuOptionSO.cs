using UnityEngine;
using KotB.Menus;

namespace MenuSystem
{
    [CreateAssetMenu(fileName = "New MenuOption", menuName = "Menu Option")]
    public class MenuOptionSO : ScriptableObject, IMenuSelection
    {
        [SerializeField] private string menuText;

        public string GetMenuText()
        {
            return menuText;
        }

        public string GetMenuKey()
        {
            return GetMenuText();
        }        
    }
}
