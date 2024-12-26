using UnityEngine;
using KotB.Items;

namespace KotB.Testing
{
    public class EstimateBallInBounds : MonoBehaviour
    {
        [SerializeField] private SkillsSO aiskills;

        private Ball ball;
        private float skillLevelMax = 10;
        private float estimateRange;

        private void Awake() {
            ball = GetComponent<Ball>();
            estimateRange = ball.BallInfo.BallRadius * 2;
        }

        public void CheckJudgement() {
            // from AI DigReadyState.cs
            // get a random value on the skill level scale
            float randValue = Random.value * skillLevelMax;
            // skill check
            bool accurateEstimate = randValue <= aiskills.InBoundsJudgement;
            // get actual in bounds value
            bool actualInBounds = ball.BallInfo.IsInBounds(ball.BallInfo.Position);
            // check if it's close
            bool closeInBounds = (Mathf.Abs(ball.BallInfo.Position.x) >= 8 - estimateRange &&
                Mathf.Abs(ball.BallInfo.Position.x) <= 8 + estimateRange) ||
                (Mathf.Abs(ball.BallInfo.Position.z) >= 4 - estimateRange &&
                Mathf.Abs(ball.BallInfo.Position.z) <= 4 + estimateRange);

            //            ↓ if it's close                                                         ↓ not close - anyone knows the right result
            bool result = closeInBounds ? (accurateEstimate ? actualInBounds : !actualInBounds) : actualInBounds;
            //                             ↑ if passed skill chk,  correct ↑    ↑ otherwise, wrong
            Debug.Log($"Ball position: {ball.BallInfo.Position} ({(actualInBounds ? "in" : "out of")} bounds)\nEstimate: {randValue} <= {aiskills.InBoundsJudgement} is {accurateEstimate}\nClose:{closeInBounds} ? (Accurate:{accurateEstimate} ? Actual:{actualInBounds} : Opp:{!actualInBounds}) : Actual:{actualInBounds} -> {(closeInBounds ? (accurateEstimate ? actualInBounds : !actualInBounds) : actualInBounds)}");
        }
    }
}
