using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class MatchEndState : MatchBaseState
    {
        public MatchEndState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter()
        {
            Debug.Log("This will change scenes to the post game stats, highlights, then usher the player to the appropriate next screen: main menu for exhibition, league home for league, etc");
        }
    }
}
