using System;
using UnityEngine;

public class CollisionTriggerReporter : MonoBehaviour
{
    [SerializeField] private bool deactivateAfterTrigger = true;

    public event Action<Collider> Triggered;

    private Collider triggerCollider;
    private bool isActive;

    private void Awake() {
        triggerCollider = GetComponent<Collider>();

        Debug.Assert(triggerCollider != null, $"No collider on game object {gameObject.name} where a CollisionReporter has been assigned.");
    }

    private void OnTriggerEnter(Collider other) {
        if (isActive) {
            Triggered?.Invoke(other);
            if (deactivateAfterTrigger) {
                isActive = false;
            }
        }
    }

    public bool Active {
        get => isActive;
        set => isActive = value;
    }
    public bool DeactivateAfterTrigger {
        get => deactivateAfterTrigger;
        set => deactivateAfterTrigger = value;
    }
    public Collider TriggerCollider => triggerCollider;
}
