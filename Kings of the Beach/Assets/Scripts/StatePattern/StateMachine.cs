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

        public void AddProfile(TransitionProfile newProfile) {
            int i = profiles.IndexOf(newProfile);

            if (i == -1) {
                profiles.Add(newProfile);
            } else {
                profiles[i] = newProfile;
            }

            SetActiveProfile(newProfile);
        }

        public void SetActiveProfile(TransitionProfile profile) {
            if (profiles.Contains(profile)) {
                currentProfile = profile;
            } else {
                UnityEngine.Debug.LogAssertion($"No such profile {profile}");
            }
        }
    }
}
