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
        protected bool powerMeterIsActive;
        private bool powerMeterIsIncreasing;

        public override void Enter() {
            Debug.Log("Entering the Player Serve state.");

            player.ServeCameraPriority.Value = 10;
            player.MainCameraPriority.Value = 0;
            player.UpdateCameraPriorty.Raise();

            player.transform.position = new Vector3((player.CourtSideLength + player.transform.localScale.x * .5f) * player.CourtSide, 0.01f, 0f);
            player.InputReader.bumpEvent += OnInteract;
        }

        public override void Exit() {
            player.InputReader.bumpEvent -= OnInteract;

            Debug.Log("Exiting the Player Serve State.");
        }

        public override void Update() {
            ValidateMovement();
            UpdatePowerMeter();
        }

        private void ValidateMovement() {
            if (
                player.MoveInput.x * player.CourtSide > 0 && player.transform.position.z >= player.CourtSideLength / 2 - player.transform.localScale.z / 2 ||
                player.MoveInput.x * player.CourtSide < 0 && player.transform.position.z <= -player.CourtSideLength / 2 + player.transform.localScale.z / 2
            ) {
                player.MoveDir = Vector3.zero;
            } else {
                player.MoveDir = new Vector3(0, 0, player.MoveInput.x * player.CourtSide);
            }
        }

        private void UpdatePowerMeter() {
            if (powerMeterIsActive) {
                float servePowerStep;
                if (powerMeterIsIncreasing) {
                    servePowerStep = (servePowerFillSpeedMax - servePowerFillSpeedMin) / (player.SkillLevelMax - 1) * (player.SkillLevelMax - player.Skills.Serving) + servePowerFillSpeedMin;
                } else {
                    servePowerStep = -((servePowerDrainSpeedMax - servePowerDrainSpeedMin) / (player.SkillLevelMax - 1) * (player.SkillLevelMax - player.Skills.Serving) + servePowerDrainSpeedMin);
                }
                player.ServePowerValue.Value += servePowerStep * Time.deltaTime;
                player.ServePowerValue.Value = Mathf.Clamp01(player.ServePowerValue.Value);
                if (player.ServePowerValue.Value == 1) powerMeterIsIncreasing = false;
                if (player.ServePowerValue.Value == 0) StopServeMeter();
            }
        }

        protected void StartServeMeter() {
            player.ServePowerValue.Value = 0;
            powerMeterIsIncreasing = true;
            powerMeterIsActive = true;
            player.ShowServePowerMeter.Raise();
        }

        protected void StopServeMeter() {
            player.HideServePowerMeter.Raise();
            powerMeterIsActive = false;
            Debug.Log("Power is " + player.ServePowerValue.Value);
        }

        private void OnInteract() {
            if (!powerMeterIsActive) {
                StartServeMeter();
            } else {
                StopServeMeter();
            }
        }
    }
}
