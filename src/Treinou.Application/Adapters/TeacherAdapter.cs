using Treinou.Application.UseCases.Teacher.CreateTeacher;
using Treinou.Application.UseCases.Teacher.Common;
using Treinou.Domain.Entities;
using Treinou.Domain.Factories;

namespace Treinou.Application.Adapters
{
    public static class TeacherAdapter
    {
        public static Teacher ToEntity(CreateTeacherInput input)
        {
            // Use Factory Method pattern to create ValueObjects with validation
            var email = ValueObjectFactory.CreateEmail(input.Email);
            var cpf = ValueObjectFactory.CreateCPF(input.CPF);
            var cref = ValueObjectFactory.CreateCREF(input.CREF);
            var phoneNumber = ValueObjectFactory.CreatePhoneNumber(input.PhoneNumber);

            // Create the Teacher entity
            var teacher = new Teacher(
                input.Name,
                email,
                cpf,
                cref,
                input.Description,
                phoneNumber,
                input.BirthDate,
                DateTime.UtcNow
            );

            teacher.UserId = input.UserId;

            return teacher;
        }


        public static TeacherModelOutput ToOutput(Teacher teacher)
        {
            return TeacherModelOutput.FromTeacher(teacher);
        }
    }
}
