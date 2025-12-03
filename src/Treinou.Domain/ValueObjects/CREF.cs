using Treinou.Domain.SeedWork;

namespace Treinou.Domain.ValueObjects
{
    public class CREF : ValueObject
    {
        public string Number { get; set; }

        public CREF(string number)
            => Number = number;
        

        public override bool Equals(ValueObject? other)
            => other is CREF cref && Number == cref.Number;

        protected override int GetCustomHashCode()
            => HashCode.Combine(Number);
    }
}
