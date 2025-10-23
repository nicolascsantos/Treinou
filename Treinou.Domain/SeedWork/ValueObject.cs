namespace Treinou.Domain.SeedWork
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public bool Equals(ValueObject? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.GetType() == this.GetType() && Equals((ValueObject)other);
        }
    }
}
