using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class InPlayState : MatchBaseState
    {
        public InPlayState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.InputReader.EnableGameplayInput();
        }
    }
}
