using UnityEngine;

namespace KotB.Stats
{
    [CreateAssetMenu(fileName = "New AthleteStats", menuName = "Game/Athlete Stats")]
    public class AthleteStatsSO : ScriptableObject
    {
        [SerializeField] private int serveAttempts;
        [SerializeField] private int serveAces;
        [SerializeField] private int serveErrors;
        [SerializeField] private int blockAttempts;
        [SerializeField] private int blocks;
        [SerializeField] private int blockPoints;
        [SerializeField] private int blockErrors;

        // ---- PROPERTIES ----
        public int ServeAttempts { get => serveAttempts; set => serveAttempts = value; }
        public int ServeAces { get => serveAces; set => serveAces = value; }
        public int ServeErrors { get => serveErrors; set => serveErrors = value; }
        public int BlockAttempts { get => blockAttempts; set => blockAttempts = value; }
        public int Blocks { get => blocks; set => blocks = value; }
        public int BlockPoints { get => blockPoints; set => blockPoints = value; }
        public int BlockErrors { get => blockErrors; set => blockErrors = value; }
    }
}
