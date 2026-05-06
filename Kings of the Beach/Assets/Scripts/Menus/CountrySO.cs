using UnityEngine;

namespace KotB.Menus.Alt
{
    [CreateAssetMenu(fileName = "New Country", menuName = "Menu/Country")]
    public class CountrySO : ScriptableObject, IMenuDisplayable
    {
        [SerializeField] private string displayName;
        [SerializeField] private string abbreviation;
        [SerializeField] private Sprite flag;

        public string DisplayName => displayName;
        public string Abbreviation => abbreviation;
        public Sprite Flag => flag;
    }
}
