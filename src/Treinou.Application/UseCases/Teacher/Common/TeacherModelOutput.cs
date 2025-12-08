namespace Treinou.Application.UseCases.Teacher.Common
{
    public class TeacherModelOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string CPF { get; set; }

        public string CREF { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public TeacherModelOutput(
            Guid id,
            string name,
            string email,
            string cpf,
            string cref,
            string description,
            string phoneNumber,
            DateTime birthDate,
            DateTime createdAt
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
            CreatedAt = createdAt;
        }

        public static TeacherModelOutput FromTeacher(Domain.Entities.Teacher teacher)
        {
            return new TeacherModelOutput(
                teacher.Id,
                teacher.Name,
                teacher.Email.Address,
                teacher.CPF.Number,
                teacher.CREF.Number,
                teacher.Description,
                teacher.PhoneNumber.Number,
                teacher.BirthDate,
                teacher.CreatedAt
            );
        }
    }
}
