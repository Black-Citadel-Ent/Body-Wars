namespace BaseItems
{
    public interface ILife
    {
        public enum DamageType { Energy, Electric, Impact }

        public void ApplyDamage(float amount, DamageType type, float direction = 0);
    }
}