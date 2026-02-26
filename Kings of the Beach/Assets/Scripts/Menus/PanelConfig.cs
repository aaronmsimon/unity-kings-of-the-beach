using System;
using System.Collections.Generic;

namespace KotB.Menus.Alt
{
    public class PanelConfig
    {
        public MenuPanel Panel;
        public Func<List<IMenuDisplayable>> LoadValues;
        public Action<IMenuDisplayable> OnSelectionChanged; // null if static
    }
}
