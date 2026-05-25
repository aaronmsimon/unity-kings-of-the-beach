using UnityEngine;
using KotB.Match;
using System.Collections.Generic;

namespace KotB.StatePattern.MatchStates
{
    public class PostPointState : MatchBaseState
    {
        public PostPointState(MatchManager matchManager) : base(matchManager) { }

        private float timeUntilChangeState;

        public override void Enter() {
            timeUntilChangeState = matchManager.TimeInPostPoint;
        }

        public override void Update() {
            timeUntilChangeState -= Time.deltaTime;

            if (timeUntilChangeState < 0) {
                matchManager.InvokePostPointComplete();
            }
        }
    }
}
