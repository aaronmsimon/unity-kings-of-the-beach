using System;
using UnityEngine;

public class CollisionTriggerReporter : MonoBehaviour
{
    public event Action Triggered;

    private Collider trigger;

    private void Awake() {
        trigger = GetComponent<Collider>();

        Debug.Assert(trigger != null, $"No collider on game object {gameObject.name} where a CollisionReporter has been assigned.");
    }

    private void OnTriggerEnter(Collider other) {
        Triggered?.Invoke();
    }
}
