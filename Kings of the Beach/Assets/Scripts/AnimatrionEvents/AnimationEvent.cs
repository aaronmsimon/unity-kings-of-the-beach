using System;
using UnityEngine.Events;

namespace adammyhre.ImprovedUnityAnimationEvents {
    [Serializable]
    public class AnimationEvent {
        public string eventName;
        public UnityEvent OnAnimationEvent;
    }
}