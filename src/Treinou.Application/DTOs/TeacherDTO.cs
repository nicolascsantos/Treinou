using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.DTOs
{
    public sealed class TeacherDTO
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string CPF { get; private set; }

        public string CREF { get; private set; }

        public string Description { get; private set; }

        public string PhoneNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public TeacherDTO(
            Guid id,
            string name,
            string email,
            string cpf,
            string cref,
            string description,
            string phoneNumber,
            DateTime birthDate
        )
        {
            Id = id;
            Name = name;
            Email = email;
            CPF = cpf;
            CREF = cref;
            Description = description;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
        }

        public static TeacherDTO FromTeacher(Entity.Teacher teacher)
            => new(
                teacher.Id,
                teacher.Name,
                teacher.Email.Address,
                teacher.CPF.Number,
                teacher.CREF.Number,
                teacher.Description,
                teacher.PhoneNumber.Number,
                teacher.BirthDate
            );
    }
}
