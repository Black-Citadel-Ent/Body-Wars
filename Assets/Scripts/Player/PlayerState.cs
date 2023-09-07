namespace Player
{
    public abstract class PlayerState
    {
        protected PlayerContext Context;

        public virtual void BeginState(PlayerContext context)
        {
            Context = context;
        }
        
        public virtual void Update() { }
        public virtual void EndState() { }
    }
}