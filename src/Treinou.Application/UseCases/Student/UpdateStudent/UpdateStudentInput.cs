using MediatR;
using Treinou.Application.UseCases.Student.Common;

namespace Treinou.Application.UseCases.Student.UpdateStudent
{
    public class UpdateStudentInput : IRequest<StudentModelOutput>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string CPF { get; set; }

        public string PhoneNumber { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public string? UserId { get; set; }

        public UpdateStudentInput(
            Guid id,
            string name,
            string email,
            string cpf,
            string phoneNumber,
            double weight,
            double height,
            string? userId = null
        )
        {
            Id = id;
            Name = name;
            Email = email;
            CPF = cpf;
            PhoneNumber = phoneNumber;
            Weight = weight;
            Height = height;
            UserId = userId;
        }
    }
}
