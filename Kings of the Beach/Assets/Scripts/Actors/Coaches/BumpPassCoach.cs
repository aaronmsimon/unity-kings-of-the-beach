using UnityEngine;

namespace KotB.Actors
{
    [System.Serializable]
    public class BumpPassCoach : CoachAction
    {
        [SerializeField] private Athlete passRecipient;

        public override void Execute()
        {
            if (passRecipient != null) coach.Pass(coach.CalculatePassTarget(passRecipient), 7, 1.75f);
        }
    }
}
