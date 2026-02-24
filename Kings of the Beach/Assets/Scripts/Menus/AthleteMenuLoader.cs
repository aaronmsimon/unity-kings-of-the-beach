using System.Collections.Generic;
using UnityEngine;
using KotB.Items;

namespace KotB.Menus.Alt
{
    public static class AthleteMenuLoader
    {
        private const string CountriesPath = "Athletes/Olympics 2024/Male";
        private const string OutfitTopPath = "Clothes/Male/Shirt";
        private const string OutfitBottomPath = "Clothes/Male/Shorts";

        public static List<IMenuDisplayable> LoadCountries() {
            return LoadAs<CountrySO>(CountriesPath);
        }

        public static List<IMenuDisplayable> LoadAthletes(string country) {
            return LoadAs<SkillsSO>($"{CountriesPath}/{country}");
        }

        public static List<IMenuDisplayable> LoadOutfitTops() {
            return LoadAs<MaterialSO>(OutfitTopPath);
        }

        public static List<IMenuDisplayable> LoadOutfitBottoms() {
            return LoadAs<MaterialSO>(OutfitBottomPath);
        }

        public static int FindDefaultIndex(List<IMenuDisplayable> values, IMenuDisplayable target) {
            if (target == null) return 0;
            int index = values.IndexOf(target);
            return index >= 0 ? index : 0;
        }

        private static List<IMenuDisplayable> LoadAs<T>(string path) where T : ScriptableObject, IMenuDisplayable {
            var results = Resources.LoadAll<T>(path);
            return new List<IMenuDisplayable>(results);
        }
    }
}
