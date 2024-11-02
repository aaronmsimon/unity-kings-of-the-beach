using UnityEngine;
using KotB.Match;

namespace KotB.StatePattern.MatchStates
{
    public class ServeState : MatchBaseState
    {
        public ServeState(MatchManager matchManager) : base(matchManager) { }

        public override void Enter() {
            matchManager.InputReader.EnableGameplayInput();
            matchManager.BallInfo.GiveBall(matchManager.MatchInfo.GetServer());
            matchManager.BallInfo.BallServed += OnBallServed;
        }

        private void OnBallServed() {
            matchManager.StateMachine.ChangeState(matchManager.InPlayState);
        }
    }
}
