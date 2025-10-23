namespace Treinou.Domain.ValueObjects
{
    public class CPF : SeedWork.ValueObject
    {
        public string Number { get; set; }

        public CPF(string number)
        {
            Number = number;
        }
    }
}
