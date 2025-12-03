using Treinou.Domain.SeedWork;

namespace Treinou.Domain.ValueObjects
{
    public class CPF : ValueObject
    {
        public string Number { get; set; }

        public CPF(string number)
            => Number = number;


        public override bool Equals(ValueObject? other)
            => other is CPF cpf && Number == cpf.Number;

        protected override int GetCustomHashCode()
            => HashCode.Combine(Number);
    }
}
