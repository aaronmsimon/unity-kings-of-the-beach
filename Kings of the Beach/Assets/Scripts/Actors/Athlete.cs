using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using RoboRyanTron.Unite2017.Events;

namespace KotB.Actors
{
    public abstract class Athlete : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] protected SkillsSO skills;

        [Header("Ball")]
        [SerializeField] protected BallSO ballSO;

        [Header("Settings")]
        [SerializeField] protected int courtSide;
        [SerializeField] private LayerMask obstaclesLayer;

        [Header("Values")]
        [SerializeField] private FloatVariable powerValue;

        [Header("Game Events")]
        [SerializeField] private GameEvent showPowerMeter;
        [SerializeField] private GameEvent hidePowerMeter;

        private bool canBump;
        private Ball _ball;
        private bool canUnlock;
        private float unlockTimer;
        private float unlockDelay = 0.25f;
        private float skillLevelMax = 10;
        private float servePowerFillSpeedMin = 1;
        private float servePowerFillSpeedMax = 5;
        private float servePowerDrainSpeedMin = 2;
        private float servePowerDrainSpeedMax = 6;
        protected bool powerMeterIsActive; // I think this should be only in the Player script
        private bool powerMeterIsIncreasing;

        protected Vector3 moveDir;
        protected AthleteState athleteState;
        protected float bumpTimer;
        protected Vector3 bumpTarget;

        protected enum AthleteState {
            Normal,
            Locked,
            Serve
        }

        private void Start() {
            athleteState = AthleteState.Normal;
            canBump = false;
            bumpTimer = 0;
        }

        protected virtual void Update() {
            switch (athleteState) {
                case AthleteState.Normal:
                    Move();
                    break;
                case AthleteState.Locked:
                    Bump();
                    TryUnlock();
                    break;
                case AthleteState.Serve:
                    if (powerMeterIsActive) {
                        powerValue.Value += (powerMeterIsIncreasing ?
                            (servePowerFillSpeedMax - servePowerFillSpeedMin) / (skillLevelMax - 1) * (skillLevelMax - skills.Serving) + servePowerFillSpeedMin :
                            -((servePowerDrainSpeedMax - servePowerDrainSpeedMin) / (skillLevelMax - 1) * (skillLevelMax - skills.Serving) + servePowerDrainSpeedMin))
                            * Time.deltaTime;
                        powerValue.Value = Mathf.Clamp01(powerValue.Value);
                        if (powerValue.Value == 1) powerMeterIsIncreasing = false;
                        if (powerValue.Value == 0) StopServeMeter();
                    }
                    break;
                default:
                    Debug.LogWarning("Athlete State unhandled.");
                    break;
            }
            Debug.Log(athleteState);
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                _ball = ball;
                canBump = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.TryGetComponent<Ball>(out Ball ball)) {
                canBump = false;
            }
        }

        public void SetAsServer() {
            athleteState = AthleteState.Serve;
            powerValue.Value = 0;
            StopServeMeter();
            showPowerMeter.Raise();
        }

        private void Move() {
            if (ballSO.ballState == BallState.Bump && ballSO.lastPlayerToHit != this) {
                float distanceToTarget = Vector3.Distance(transform.position, ballSO.Target);
                if (distanceToTarget <= skills.TargetLockDistance) {
                    transform.position = ballSO.Target;
                    athleteState = AthleteState.Locked;
                    canUnlock = false;
                    return;
                }
            }

            bool canMove = !Physics.Raycast(transform.position + Vector3.up * 0.5f, moveDir, out RaycastHit hit, 0.5f, obstaclesLayer);
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveDir, Color.red);
            if (canMove) {
                transform.position += moveDir * skills.MoveSpeed * Time.deltaTime;
            }

            skills.Position = transform.position;
        }

        private void Bump() {
            bumpTimer -= Time.deltaTime;
            if (canBump && bumpTimer > 0 && _ball != null) {
                _ball.Bump(bumpTarget, 12, 2);
                canUnlock = true;
                unlockTimer = unlockDelay;
                canBump = false;
                ballSO.HitsForTeam += 1;
                Debug.Log("Hits: " + ballSO.HitsForTeam);
                ballSO.lastPlayerToHit = this;
            }
        }

        private void TryUnlock() {
            if (canUnlock) {
                unlockTimer -= Time.deltaTime;
                if (unlockTimer <= 0) {
                    athleteState = AthleteState.Normal;
                }
            }
        }

        private void SetCourtSide(int courtSide) {
            this.courtSide = courtSide;
        }

        private void SwitchCourtSide() {
            courtSide *= -1;
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

        //---- EVENT LISTENERS ----
        public void OnBallHitGround() {
            athleteState = AthleteState.Normal;
        }
    }
}
