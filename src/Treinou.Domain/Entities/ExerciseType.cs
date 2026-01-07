using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Entities
{
    public class ExerciseType : AggregateRoot
    {
        public ExerciseType(string name)
        {
            Name = name;
            Validate();
        }

        public string Name { get; set; }

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

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
