using Treinou.Application.UseCases.Student.CreateStudent;
using Treinou.Application.UseCases.Student.Common;
using Treinou.Domain.Entities;
using Treinou.Domain.Factories;

namespace Treinou.Application.Adapters
{
    public static class StudentAdapter
    {
        public static Student ToEntity(CreateStudentInput input)
        {
            var email = ValueObjectFactory.CreateEmail(input.Email);
            var cpf = ValueObjectFactory.CreateCPF(input.CPF);
            var phoneNumber = ValueObjectFactory.CreatePhoneNumber(input.PhoneNumber);

            var student = new Student(
                input.Name,
                email,
                cpf,
                phoneNumber,
                input.Weight,
                input.Height,
                input.IsActive
            );

            student.TeacherId = input.TeacherId;

            return student;
        }

        public static StudentModelOutput ToOutput(Student student)
        {
            return StudentModelOutput.FromStudent(student);
        }
    }
}
