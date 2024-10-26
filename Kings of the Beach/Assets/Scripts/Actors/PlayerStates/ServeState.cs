using UnityEngine;
using KotB.Actors;

namespace KotB.StatePattern.PlayerStates
{
    public class ServeState : PlayerBaseState
    {
        public ServeState(Player player) : base(player) { }

        private float servePowerFillSpeedMin = 1;
        private float servePowerFillSpeedMax = 5;
        private float servePowerDrainSpeedMin = 2;
        private float servePowerDrainSpeedMax = 6;
        private bool serveUIIsActive;
        private bool powerMeterIsIncreasing;

        public override void Enter() {
            player.ServeCameraPriority.Value = 10;
            player.MainCameraPriority.Value = 0;
            player.UpdateCameraPriorty.Raise();
            
            player.ShowServeAim.Raise();

            player.transform.position = new Vector3((player.CourtSideLength + player.transform.localScale.x * .5f) * player.CourtSide, 0.01f, 0f);

            player.InputReader.bumpEvent += OnInteract;
        }

        public override void Exit() {
            player.InputReader.bumpEvent -= OnInteract;
        }

        public override void Update() {
            ValidateMovement();
            UpdateServeAim();
            UpdatePowerMeter();
        }

        private void ValidateMovement() {
            if (
                player.RightStickInput.x * player.CourtSide > 0 && player.transform.position.z >= player.CourtSideLength / 2 - player.transform.localScale.z / 2 ||
                player.RightStickInput.x * player.CourtSide < 0 && player.transform.position.z <= -player.CourtSideLength / 2 + player.transform.localScale.z / 2
            ) {
                player.MoveDir = Vector3.zero;
            } else {
                player.MoveDir = new Vector3(0, 0, player.RightStickInput.x * player.CourtSide);
            }
        }

        private void UpdateServeAim() {
            Vector2 aim = Helpers.CircleMappedToSquare(player.MoveInput.x, player.MoveInput.y);
            float targetZ = aim.x * 4 * player.CourtSide;
            float targetY = aim.y * 1.5f + 3.5f;
            player.ServeAimPosition.Value = new Vector3(0f, targetY, targetZ);
        }

        private void UpdatePowerMeter() {
            if (serveUIIsActive) {
                float servePowerStep;
                if (powerMeterIsIncreasing) {
                    servePowerStep = (servePowerFillSpeedMax - servePowerFillSpeedMin) / (player.SkillLevelMax - 1) * (player.SkillLevelMax - player.Skills.Serving) + servePowerFillSpeedMin;
                } else {
                    servePowerStep = -((servePowerDrainSpeedMax - servePowerDrainSpeedMin) / (player.SkillLevelMax - 1) * (player.SkillLevelMax - player.Skills.Serving) + servePowerDrainSpeedMin);
                }
                player.ServePowerValue.Value += servePowerStep * Time.deltaTime;
                player.ServePowerValue.Value = Mathf.Clamp01(player.ServePowerValue.Value);
                if (player.ServePowerValue.Value == 1) powerMeterIsIncreasing = false;
                if (player.ServePowerValue.Value == 0) DisplayServeUI(false);
            }
        }

        private void StartServeMeter() {
            player.ServePowerValue.Value = 0;
            powerMeterIsIncreasing = true;
            DisplayServeUI(true);
        }

        private void StopServeMeter() {
            DisplayServeUI(false);
            player.HideServeAim.Raise();
            player.BallInfo.BallServedEvent();
            Vector3 servePos = player.BallInfo.SkillValues.AdjustedServeDirection(player.ServeAimPosition.Value, player.Skills.Serving);
            player.BallInfo.SetServeTarget(servePos, player.ServePowerValue.Value, player);
            player.StateMachine.ChangeState(player.NormalState);
        }

        private void DisplayServeUI(bool isActive) {
            serveUIIsActive = isActive;
            if (isActive) {
                player.ShowServePowerMeter.Raise();
            } else {
                player.HideServePowerMeter.Raise();
            }
        }

        private void OnInteract() {
            if (!serveUIIsActive) {
                StartServeMeter();
            } else {
                StopServeMeter();
            }
        }
    }
}
