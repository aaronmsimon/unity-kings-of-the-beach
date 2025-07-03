using UnityEngine;
using KotB.Items;

namespace KotB.Actors
{
    public class BumpPassCoach : CoachAction
    {
        [SerializeField] private Athlete passRecipient;
        [SerializeField] private PassType passType = PassType.Bump;

        public override void Execute()
        {
            if (passRecipient != null) coach.Pass(coach.CalculatePassTarget(passRecipient), 7, 1.75f, passType);
        }
    }
}
