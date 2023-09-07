namespace Virus
{
    public abstract class VirusState
    {
        protected VirusContext Context;
        
        public virtual void BeginState(VirusContext context)
        {
            Context = context;
        }

        public virtual void Update() { }
        public virtual void Damage() { }
        public virtual void EndState() { }
    }
}