using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;
using Treinou.Domain.ValueObjects;

namespace Treinou.Domain.Entities
{
    public class Teacher : AggregateRoot
    {
        private const int NAME_MAX_LENGTH = 200;
        private const int NAME_MIN_LENGTH = 3;
        private const int DESCRIPTION_MAX_LENGTH = 200;
        private const int DESCRIPTION_MIN_LENGTH = 10;

        public Teacher(
            string name,
            Email email,
            CPF cpf,
            CREF cref,
            string description,
            PhoneNumber phoneNumber,
            DateTime birthDate,
            DateTime createdAt
        )
        {
            Name = name;
            Email = email;
            CPF = cpf;
            CREF = cref;
            Description = description;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            CreatedAt = createdAt;
            Validate();
        }

        public Teacher() {}

        public string Name { get; set; }

        public Email Email { get; set; }

        public CPF CPF { get; set; }

        public CREF CREF { get; set; }

        public string Description { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? UserId { get; set; }

        public ICollection<Student> Students { get; } = new List<Student>();

        public ICollection<Workout> Workouts { get; } = new List<Workout>();

        public void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            DomainValidation.MinLength(Name, nameof(Name), NAME_MIN_LENGTH);
            DomainValidation.MaxLength(Name, nameof(Name), NAME_MAX_LENGTH);
            DomainValidation.NotNull(Email, nameof(Email));
            DomainValidation.NotNull(CPF, nameof(CPF));
            DomainValidation.NotNull(CREF, nameof(CREF));
            DomainValidation.NotNullOrEmpty(Description, nameof(Description));
            DomainValidation.MinLength(Description, nameof(Description), DESCRIPTION_MIN_LENGTH);
            DomainValidation.MaxLength(Description, nameof(Description), DESCRIPTION_MAX_LENGTH);
            DomainValidation.NotNull(PhoneNumber, nameof(PhoneNumber));
            DomainValidation.NotNull(BirthDate, nameof(BirthDate));
        }

        public void Update(
            string name,
            Email email,
            CPF cpf,
            CREF cref,
            string description,
            PhoneNumber phoneNumber
        )
        {
            Name = name;
            Email = email;
            CPF = cpf;
            CREF = cref;
            Description = description;
            PhoneNumber = phoneNumber;
            Validate();
        }

        public void AddStudent(Student student)
            => Students.Add(student);
    }
}
