using Treinou.Domain.SeedWork;
using Treinou.Domain.Validation;
using Treinou.Domain.ValueObjects;

namespace Treinou.Domain.Entities
{
    public class Coach : AggregateRoot
    {
        public string Name { get; set; }

        public CPF CPF { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string CREF { get; set; }

        public string Phone { get; set; }

        public string Biography { get; set; }

        public string Specialty { get; set; }

        public ProfilePicture ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; }

        public Coach(
            string name,
            CPF cpf,
            DateTime dateOfBirth,
            string email,
            string cref,
            string phone,
            string biography, 
            string specialty,
            ProfilePicture profilePicture,
            DateTime createdAt
        )
        {
            Name = name;
            CPF = cpf;
            DateOfBirth = dateOfBirth;
            Email = email;
            CREF = cref;
            Phone = phone;
            Biography = biography;
            Specialty = specialty;
            ProfilePicture = profilePicture;
            CreatedAt = createdAt;
        }

        public void Validate(ValidationHandler notificationHandler) => new CoachValidator(this, notificationHandler);

        public void Update(
            string name,
            CPF cpf,
            DateTime dateOfBirth,
            string email,
            string cref,
            string phone,
            string biography,
            string specialty,
            ProfilePicture profilePicture
        )
        {
            Name = name;
            CPF = cpf;
            DateOfBirth = dateOfBirth;
            Email = email;
            CREF = cref;
            Phone = phone;
            Biography = biography;
            Specialty = specialty;
            ProfilePicture = profilePicture;
        }
    }
}
