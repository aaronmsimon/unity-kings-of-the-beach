using UnityEngine;

namespace KotB.StatePattern
{
    public interface IState
    {
        public void Enter();

        public void Update();
        
        public void Exit();

        public void OnTriggerEnter(Collider other);

        public void OnTriggerExit(Collider other);
    }
}
