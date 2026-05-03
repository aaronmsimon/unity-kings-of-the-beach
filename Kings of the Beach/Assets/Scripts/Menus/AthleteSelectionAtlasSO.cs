using System.Collections.Generic;
using UnityEngine;

namespace KotB.Menus.Alt
{
    [CreateAssetMenu(fileName = "New Athlete Selection Atlas", menuName = "Menu/Atlas/Athlete Selection")]
    public class AthleteSelectionAtlasSO : ScriptableObject
    {
        public List<InputIcons> inputIcons;

        [System.Serializable]
        public class InputSchemes
        {
            public Texture Keyboard;
            public Texture Xbox;
            public Texture PlayStation;
            public Texture Generic;
        }

        [System.Serializable]
        public class InputIcons
        {
            public string inputName;
            public InputSchemes inputSchemes;
        }
    }
}
