using MediatR;
using Treinou.Application.UseCases.Teacher.Common;

namespace Treinou.Application.UseCases.Teacher.CreateTeacher
{
    public class CreateTeacherInput : IRequest<TeacherModelOutput>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string CPF { get; set; }

        public string CREF { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string? UserId { get; set; }

        public CreateTeacherInput(
            string name,
            string email,
            string cpf,
            string cref,
            string description,
            string phoneNumber,
            DateTime birthDate,
            string? userId = null
        )
        {
            Name = name;
            Email = email;
            CPF = cpf;
            CREF = cref;
            Description = description;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            UserId = userId;
        }
    }
}
