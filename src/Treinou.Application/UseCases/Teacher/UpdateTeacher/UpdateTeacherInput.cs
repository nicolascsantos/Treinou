using MediatR;
using Treinou.Application.UseCases.Teacher.Common;

namespace Treinou.Application.UseCases.Teacher.UpdateTeacher
{
    public class UpdateTeacherInput : IRequest<TeacherModelOutput>
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string CPF { get; private set; }

        public string CREF { get; private set; }

        public string  Description { get; private set; }

        public string PhoneNumber { get; private set; }

        public string? UserId { get; private set; }

        public UpdateTeacherInput(
            Guid id,
            string name,
            string email,
            string cpf,
            string cref,
            string description,
            string phoneNumber,
            string? userId = null
        )
        {
            Id = id;
            Name = name;
            Email = email;
            CPF = cpf;
            CREF = cref;
            Description = description;
            PhoneNumber = phoneNumber;
            UserId = userId;
        }
    }
}
