using System.Collections.Generic;
using UnityEngine;

namespace KotB.Actors
{
    public class AthleteThoughts : MonoBehaviour
    {
        // Queue to store the rolling collection of 10 items
        public Queue<string> itemsQueue = new Queue<string>();
        
        private Athlete athlete;
        private const int MaxItems = 10; // Maximum number of items to store

        private void Awake() {
            athlete = GetComponent<Athlete>();
        }

        private void OnEnable() {
            // athlete.Thought += OnThought;
        }

        private void OnDisable() {
            // athlete.Thought -= OnThought;
        }

        // Method to add an item to the queue
        public void AddThought(string item)
        {
            if (itemsQueue.Count >= MaxItems)
            {
                itemsQueue.Dequeue();  // Remove the oldest item when queue is full
            }
            itemsQueue.Enqueue(item);
        }

        public void ClearQueue() {
            itemsQueue.Clear();
        }

        public string CurrentState {
            get {
                if (athlete != null) {
                    return athlete.StateMachine.CurrentState.GetType().Name;
                } else {
                    return "No State";
                }
            }
        }

        private void OnThought(string thought) {
            AddThought(thought);
        }
    }
}
