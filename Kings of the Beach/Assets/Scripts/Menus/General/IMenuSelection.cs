namespace KotB.Menus
{
    public interface IMenuSelection
    {
        /// Human‑readable name shown in the ListView (e.g., "United States").
        string GetMenuText();

        /// Key used for lookups (defaults to display text if not overridden).
        /// If your folder names differ from display names, have your asset return a stable key here
        /// (e.g., "usa", "canada"). If you don't need this, you can return GetMenuText().
        string GetMenuKey();
    }
}
