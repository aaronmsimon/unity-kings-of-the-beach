using System;
using System.Collections.Generic;
using UnityEngine;
using KotB.Items;

namespace KotB.Menus
{
    public class StartMatch : MonoBehaviour
    {
        [SerializeField] private List<PlayerSelection> playerSelections;

        public void CompleteMatchSetup() {
            foreach (PlayerSelection playerSelection in playerSelections) {
                playerSelection.AthleteConfig.skills = Resources.Load<SkillsSO>($"Athletes/Olympics 2024/Male/{playerSelection.Country.MenuText}/{playerSelection.Player.MenuText}");
                playerSelection.AthleteConfig.computerControlled = false;
                playerSelection.AthleteConfig.outfit = (Outfit)Enum.Parse(typeof(Outfit), playerSelection.Outfit.MenuText.ToString());
                playerSelection.AthleteConfig.top = Resources.Load<MaterialSO>($"Athletes/Clothes/Male/Shirt/{playerSelection.Top.MenuText}");
                playerSelection.AthleteConfig.bottom = Resources.Load<MaterialSO>($"Athletes/Clothes/Male/Shorts/{playerSelection.Bottom.MenuText}");
            }
        }
    }
}
