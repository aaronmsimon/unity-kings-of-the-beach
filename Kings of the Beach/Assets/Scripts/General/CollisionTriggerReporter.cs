using System;
using KotB.Actors;
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
        AI ai = GetComponentInParent<AI>();
        if (ai != null) Debug.Log($"{ai.Skills.AthleteName}'s {gameObject.name} collider has been triggered at {Time.time}. Is Active? {isActive}");
        if (isActive) Triggered?.Invoke(other);
    }

    public bool Active {
        get => isActive;
        set => isActive = value;
    }
    public Collider TriggerCollider => triggerCollider;
}
