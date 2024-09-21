namespace KotB.StatePattern
{
    public interface IState
    {
        public void Enter();

        public void Update();
        
        public void Exit();
    }
}
