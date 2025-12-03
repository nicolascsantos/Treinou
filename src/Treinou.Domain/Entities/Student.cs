using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;
using Treinou.Domain.ValueObjects;

namespace Treinou.Domain.Entities
{
    public class Student : AggregateRoot
    {
        private const int NAME_MAX_LENGTH = 200;
        private const int NAME_MIN_LENGTH = 3;

        public string Name { get; set; }

        public Email Email { get; set; }

        public CPF CPF { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid TeacherId { get; set; }

        public Teacher Teacher { get; set; } = null!;

        public ICollection<Workout> Workouts { get; } = new List<Workout>();

        public Student(
            string name,
            Email email,
            CPF cpf,
            PhoneNumber phoneNumber,
            double weight,
            double height,
            bool isActive = true
        )
        {
            Name = name;
            Email = email;
            CPF = cpf;
            PhoneNumber = phoneNumber;
            Weight = weight;
            Height = height;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
            Validate();
        }


        public void Validate()
        {
            DomainValidation.NotNull(Name, nameof(Name));
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            DomainValidation.MinLength(Name, nameof(Name), NAME_MIN_LENGTH);
            DomainValidation.MaxLength(Name, nameof(Name), NAME_MAX_LENGTH);
            DomainValidation.NotNull(Email, nameof(Email));
            DomainValidation.NotNullOrEmpty(Email.Address, nameof(Email.Address));
            DomainValidation.NotNull(CPF, nameof(CPF));
            DomainValidation.NotNullOrEmpty(CPF.Number, nameof(CPF.Number));
            DomainValidation.NotNull(PhoneNumber, nameof(PhoneNumber));
            DomainValidation.NotNullOrEmpty(PhoneNumber.Number, nameof(PhoneNumber.Number));
            DomainValidation.NotNull(Weight, nameof(Weight));
            DomainValidation.NotNull(Height, nameof(Height));
        }

        public void Update(
            string name, 
            Email email,
            CPF cpf,
            PhoneNumber phoneNumber,
            double weight,
            double height)
        {
            Name = name;
            Email = email;
            CPF = cpf;
            PhoneNumber = phoneNumber;
            Weight = weight;
            Height = height;
            Validate();
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
    }
}
