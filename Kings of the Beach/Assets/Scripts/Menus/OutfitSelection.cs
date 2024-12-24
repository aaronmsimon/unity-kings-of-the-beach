using System;
using UnityEngine;

namespace KotB.Menus
{
    public class OutfitSelection : UIGroupSelect
    {
        public override void LoadSelections()
        {
            groupItems = Enum.GetNames(typeof(Outfit));

            UpdateDisplay();
            RaiseSelectionChangedEvent();
        }
    }
}
