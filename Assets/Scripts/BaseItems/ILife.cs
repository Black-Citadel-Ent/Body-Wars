namespace BaseItems
{
    public interface ILife
    {
        public enum DamageType { Energy, Electric }

        public void ApplyDamage(float amount, DamageType type);
    }
}