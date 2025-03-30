using System;
using UnityEngine;

public class CollisionTriggerReporter : MonoBehaviour
{
    public event Action Triggered;

    private Collider trigger;
    private bool isActive;

    private void Awake() {
        trigger = GetComponent<Collider>();

        Debug.Assert(trigger != null, $"No collider on game object {gameObject.name} where a CollisionReporter has been assigned.");
    }

    private void OnTriggerEnter(Collider other) {
        if (isActive) Triggered?.Invoke();
    }

    public bool Active {
        get => isActive;
        set => isActive = value;
    }
    public Collider Collider => trigger;
}
