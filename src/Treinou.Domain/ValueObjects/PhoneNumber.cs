using Treinou.Domain.SeedWork;

namespace Treinou.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Number { get; set; }

        public PhoneNumber(string number)
            => Number = number;


        public override bool Equals(ValueObject? other)
            => other is PhoneNumber phoneNumber && Number == phoneNumber.Number;

        protected override int GetCustomHashCode()
            => HashCode.Combine(Number);
    }
}
