using UnityEngine;
using KotB.StatePattern;

namespace KotB.Actors.PlayerStates
{
    public class ServeState : State
    {
        private InputReader inputReader;
        private Athlete player;
        private SkillsSO skills;

        public ServeState(InputReader inputReader, Athlete player) {
            this.inputReader = inputReader;
            this.player = player;
            skills = player.Skills;
        }

        public override void Enter() {
            Debug.Log("Entering the Player Serve state.");

            // inputReader.interactEvent += OnInteract;
        }

        public override void Exit() {
            // inputReader.interactEvent -= OnInteract;

            Debug.Log("Exiting the Player Serve State.");
        }

        public override void Update() {
            //<athlete>
            if (powerMeterIsActive) {
                powerValue.Value += (powerMeterIsIncreasing ?
                    (servePowerFillSpeedMax - servePowerFillSpeedMin) / (skillLevelMax - 1) * (skillLevelMax - skills.Serving) + servePowerFillSpeedMin :
                    -((servePowerDrainSpeedMax - servePowerDrainSpeedMin) / (skillLevelMax - 1) * (skillLevelMax - skills.Serving) + servePowerDrainSpeedMin))
                    * Time.deltaTime;
                powerValue.Value = Mathf.Clamp01(powerValue.Value);
                if (powerValue.Value == 1) powerMeterIsIncreasing = false;
                if (powerValue.Value == 0) StopServeMeter();
            }
            //</athlete>
        }

        private void OnInteract() {
            // changeToServeState.Raise();
        }

        // From Athlete
        [Header("Values")]
        [SerializeField] private RoboRyanTron.Unite2017.Variables.FloatVariable powerValue;

        [Header("Game Events")]
        [SerializeField] private RoboRyanTron.Unite2017.Events.GameEvent showPowerMeter;
        [SerializeField] private RoboRyanTron.Unite2017.Events.GameEvent hidePowerMeter;

        private float skillLevelMax = 10;
        private float servePowerFillSpeedMin = 1;
        private float servePowerFillSpeedMax = 5;
        private float servePowerDrainSpeedMin = 2;
        private float servePowerDrainSpeedMax = 6;
        protected bool powerMeterIsActive; // I think this should be only in the Player script
        private bool powerMeterIsIncreasing;
        
        public void SetAsServer() {
            // athleteState = AthleteState.Serve;
            powerValue.Value = 0;
            StopServeMeter();
            showPowerMeter.Raise();
        }

        protected void StartServeMeter() {
            powerValue.Value = 0;
            powerMeterIsIncreasing = true;
            powerMeterIsActive = true;
        }

        protected void StopServeMeter() {
            powerMeterIsActive = false;
            Debug.Log("Power is " + powerValue.Value);
            hidePowerMeter.Raise();
        }

        private void OnBump() {
            if (!powerMeterIsActive) {
                StartServeMeter();
            } else {
                StopServeMeter();
            }
        }
    }
}
