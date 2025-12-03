using Treinou.Domain.SeedWork;

namespace Treinou.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Address { get; set; }

        public Email(string address)
            => Address = address;


        public override bool Equals(ValueObject? other)
            => other is Email email && Address == email.Address;

        protected override int GetCustomHashCode()
            => HashCode.Combine(Address);
    }
}
