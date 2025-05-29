using UnityEngine;

namespace KotB.StatePattern
{
    [CreateAssetMenu(fileName = "State Transition", menuName = "State Machine/Transition")]
    public class TransitionSO : ScriptableObject
    {
        public BaseState From;
        public BaseState To;
        public IPredicate Condition { get; }
    }
}