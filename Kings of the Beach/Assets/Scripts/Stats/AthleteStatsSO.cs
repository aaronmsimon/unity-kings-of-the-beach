using UnityEngine;

namespace KotB.Stats
{
    [CreateAssetMenu(fileName = "New AthleteStats", menuName = "Game/Athlete Stats")]
    public class AthleteStatsSO : ScriptableObject
    {
        [SerializeField] private int serveAttempts;
        [SerializeField] private int serveAces;
        [SerializeField] private int serveErrors;

        // ---- PROPERTIES ----
        public int ServeAttempts { get => serveAttempts; set => serveAttempts = value; }
        public int ServeAces { get => serveAces; set => serveAces = value; }
        public int ServeErrors { get => serveErrors; set => serveErrors = value; }
    }
}
