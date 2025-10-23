namespace Treinou.Domain.Validation.Coach
{
    public class CoachValidator : Validator
    {
        private readonly Entities.Coach _coach;

        private const int NAME_MIN_LENGTH = 3;
        private const int NAME_MAX_LENGTH = 255;

        public CoachValidator(Entities.Coach coach, ValidationHandler handler) : base(handler)
            => _coach = coach;


        public override void Validate()
        {
            ValidateName();
        }

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(_coach.Name))
                _handler.HandleError("Name should not be null or empty.");

            if (_coach.Name.Length < NAME_MIN_LENGTH)
                _handler.HandleError($"Name should have at least {NAME_MIN_LENGTH} characters.");

            if (_coach.Name.Length > NAME_MAX_LENGTH)
                _handler.HandleError($"{nameof(_coach.Name)} should be less or equal {NAME_MAX_LENGTH} characters long.");
        }
    }
}
