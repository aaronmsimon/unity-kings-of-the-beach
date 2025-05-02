using System;
using UnityEngine;

public class CollisionTriggerReporter : MonoBehaviour
{
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
            isActive = false;
        }
    }

    public bool Active {
        get => isActive;
        set => isActive = value;
    }
    public Collider TriggerCollider => triggerCollider;
}
