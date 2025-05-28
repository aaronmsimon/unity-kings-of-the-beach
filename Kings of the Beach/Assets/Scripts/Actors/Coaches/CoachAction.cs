namespace KotB.Actors
{
    [System.Serializable]
    public abstract class CoachAction
    {
        public abstract void Execute(Coach coach);
    }
}
