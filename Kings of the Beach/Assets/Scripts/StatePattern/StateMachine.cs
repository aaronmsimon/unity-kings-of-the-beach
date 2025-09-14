using System;
using System.Collections.Generic;

namespace KotB.StatePattern
{
    public class StateMachine
    {
        private TransitionProfile currentProfile;
        private List<TransitionProfile> profiles = new List<TransitionProfile>();

        public event Action<IState> StateChanged;

        public void ChangeState(IState newState) {
            currentProfile.ChangeState(newState);
            StateChanged?.Invoke(newState);
        }

        public void Update() {
            var transition = currentProfile.GetTransition();
            if (transition != null) {
                ChangeState(transition.To);
            }
            currentProfile.Update();
        }

        public void SetState(IState newState) {
            currentProfile.SetState(newState);
            StateChanged?.Invoke(newState);
        }

        public void AddProfile(TransitionProfile newProfile, bool setActive = false) {
            int i = profiles.FindIndex(p => p.Name == newProfile.Name);

            if (i == -1) {
                profiles.Add(newProfile);
            } else {
                profiles[i] = newProfile;
            }

            if (setActive) SetActiveProfile(newProfile);
        }

        public void SetActiveProfile(TransitionProfile profile) {
            if (profiles.Contains(profile)) {
                currentProfile = profile;
                currentProfile.ActivateProfile();
                StateChanged?.Invoke(currentProfile.CurrentState);
            } else {
                UnityEngine.Debug.LogAssertion($"{profile} does not exist.");
            }
        }

        public void SetActiveProfile(string profileName) {
            int i = profiles.FindIndex(p => p.Name == profileName);

            if (i >= 0) {
                currentProfile = profiles[i];
                currentProfile.ActivateProfile();
                StateChanged?.Invoke(currentProfile.CurrentState);
            } else {
                UnityEngine.Debug.LogAssertion($"{profileName} does not exist.");
            }
        }

        public TransitionProfile CurrentProfile => currentProfile;
        public IState CurrentState => currentProfile.CurrentState;
    }
}
