namespace Bacteria
{
    public abstract class BacteriaState
    {
        protected BacteriaContext Context;

        public virtual void BeginState(BacteriaContext context)
        {
            Context = context;
        }
        
        public virtual void Update() { }
        public virtual void EndState() { }
    }
}