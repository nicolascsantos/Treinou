using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Entities
{
    public class Workout : AggregateRoot
    {
        public string Name { get; private set; }

        public Guid TeacherId { get; private set; }
        public Teacher Teacher { get; private set; }

        public Guid StudentId { get; private set; }
        public Student Student { get; private set; }

        public bool IsActive { get; private set; }

        public Workout(
            string name,
            Guid teacherId,
            Guid studentId,
            bool isActive = true
        )
        {
            Name = name;
            TeacherId = teacherId;
            StudentId = studentId;
            IsActive = isActive;
        }

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
        }

        public void Update(string name)
        {
            Name = name;
        }
    }
}
