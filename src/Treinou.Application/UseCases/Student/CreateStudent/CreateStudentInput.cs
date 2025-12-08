using MediatR;
using Treinou.Application.UseCases.Student.Common;

namespace Treinou.Application.UseCases.Student.CreateStudent
{
    public class CreateStudentInput : IRequest<StudentModelOutput>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string CPF { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public Guid TeacherId { get; set; }

        public bool IsActive { get; set; } = true;

        public CreateStudentInput(
            string name,
            string email,
            string cpf,
            string phoneNumber,
            DateTime birthDate,
            double weight,
            double height,
            Guid teacherId,
            bool isActive = true
        )
        {
            Name = name;
            Email = email;
            CPF = cpf;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Weight = weight;
            Height = height;
            TeacherId = teacherId;
            IsActive = isActive;
        }
    }
}
