using UnityEngine;
using KotB.Actors;
using KotB.StatePattern.AIStates;
using KotB.StatePattern.PlayerStates;
using KotB.StatePattern;

namespace KotB.Testing
{
    public class SetAthleteState : MonoBehaviour
    {
        private enum AthleteType {
            player,
            ai,
            none
        }

        private enum AthleteState {
            playerNormal,
            playerLock,
            playerServe,
            playerPostPoint,
            aiServe,
            aiDefense,
            aiOffense,
            aiDigReady,
            aiPostPoint,
            aiNonServe
        }

        [SerializeField] private AthleteState athleteState;

        private Athlete athlete;
        private AthleteType athleteType;

        // Player States
        private NormalState playerNormalState;
        private LockState playerLockState;
        private StatePattern.PlayerStates.ServeState playerServeState;
        private StatePattern.PlayerStates.PostPointState playerPostPointState;

        // AI States
        private StatePattern.AIStates.ServeState aiServeState;
        private DefenseState aiDefenseState;
        private OffenseState aiOffenseState;
        private DigReadyState aiDigReadyState;
        private StatePattern.AIStates.PostPointState aiPostPointState;
        private NonServeState aiNonServeState;

        private void Awake() {
            athlete = GetComponent<Athlete>();
            Player player = GetComponent<Player>();
            AI ai = GetComponent<AI>();

            if (player != null) {
                athleteType = AthleteType.player;
                playerNormalState = new NormalState(player);
                playerLockState = new LockState(player);
                playerServeState = new StatePattern.PlayerStates.ServeState(player);
                playerPostPointState = new StatePattern.PlayerStates.PostPointState(player);

            } else if (ai != null) {
                athleteType = AthleteType.ai;
                aiServeState = new StatePattern.AIStates.ServeState(ai);
                aiDefenseState = new DefenseState(ai);
                aiOffenseState = new OffenseState(ai);
                aiDigReadyState = new DigReadyState(ai);
                aiPostPointState = new StatePattern.AIStates.PostPointState(ai);
                aiNonServeState = new NonServeState(ai);
            } else {
                athleteType = AthleteType.none;
            }

        }
        
        public void SetState() {
            IState state;
            if (athleteType == AthleteType.player) {
                state = playerPostPointState;
            } else if (athleteType == AthleteType.ai) {
                state = aiPostPointState;
            } else {
                return;
            }

            switch (athleteState) {
                case AthleteState.playerNormal:
                    state = playerNormalState;
                    break;
                case AthleteState.playerLock:
                    state = playerLockState;
                    break;
                case AthleteState.playerServe:
                    state = playerServeState;
                    break;
                case AthleteState.aiServe:
                    state = aiServeState;
                    break;
                case AthleteState.aiDefense:
                    state = aiDefenseState;
                    break;
                case AthleteState.aiOffense:
                    state = aiOffenseState;
                    break;
                case AthleteState.aiDigReady:
                    state = aiDigReadyState;
                    break;
                case AthleteState.aiNonServe:
                    state = aiNonServeState;
                    break;
            }

            athlete.StateMachine.ChangeState(state);
        }
    }
}
