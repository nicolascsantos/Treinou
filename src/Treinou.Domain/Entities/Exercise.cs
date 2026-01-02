using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Entities
{
    public class Exercise : AggregateRoot
    {
        public string Name { get; private set; }

        public string? ImageUrl { get; private set; }

        public Guid ExcerciseTypeId { get; private set; }
        public ExerciseType? ExcerciseType { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Exercise(
            string name,
            Guid exerciseTypeId,
            bool isActive = true,
            string? imageUrl = null
        )
        {
            Name = name;
            ExcerciseTypeId = exerciseTypeId;
            IsActive = true;
            CreatedAt = DateTime.Now;
            ImageUrl = imageUrl;
            Validate();
        }

        public Exercise() {}

        public void Activate()
        {
            IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            DomainValidation.NotNull(ExcerciseTypeId, nameof(ExcerciseTypeId));
        }

        public void Update(string name, Guid? exerciseTypeId, string? imageUrl)
        {
            Name = name;
            ExcerciseTypeId = exerciseTypeId ?? ExcerciseTypeId;
            ImageUrl = imageUrl ?? ImageUrl;
            Validate();
        }
    }
}
