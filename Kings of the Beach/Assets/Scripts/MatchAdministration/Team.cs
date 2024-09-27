using UnityEngine;
using KotB.Actors;

namespace KotB.Match
{
    [System.Serializable]
    public class Team
    {
        [SerializeField] private string teamName;
        [SerializeField] private Athlete[] athletes = new Athlete[2];

        public Athlete[] Athletes {
            get {
                return athletes;
            }
        }
    }
}
