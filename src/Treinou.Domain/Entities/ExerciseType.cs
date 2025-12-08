using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Entities
{
    public class ExerciseType : Entity
    {
        public ExerciseType(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }

        public void Update(string name)
        {
            Name = name;
            Validate();
        }
    }
}
