using Treinou.Application.DTOs;

namespace Treinou.Application.UseCases.Student.Common
{
    public class StudentModelOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string CPF { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public TeacherDTO Teacher { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? UserId { get; set; }

        public StudentModelOutput(
            Guid id,
            string name,
            string email,
            string cpf,
            string phoneNumber,
            DateTime birthDate,
            double weight,
            double height,
            TeacherDTO teacher,
            bool isActive,
            DateTime createdAt,
            string? userId
        )
        {
            Id = id;
            Name = name;
            Email = email;
            CPF = cpf;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Weight = weight;
            Height = height;
            Teacher = teacher;
            IsActive = isActive;
            CreatedAt = createdAt;
            UserId = userId;
        }

        public static StudentModelOutput FromStudent(Domain.Entities.Student student)
        {
            return new StudentModelOutput(
                student.Id,
                student.Name,
                student.Email.Address,
                student.CPF.Number,
                student.PhoneNumber.Number,
                student.BirthDate,
                student.Weight,
                student.Height,
                new TeacherDTO(
                    student.Teacher.Id,
                    student.Teacher.Name,
                    student.Teacher.Email.Address,
                    student.Teacher.CPF.Number,
                    student.Teacher.CREF.Number,
                    student.Teacher.Description,
                    student.Teacher.PhoneNumber.Number,
                    student.Teacher.BirthDate),
                student.IsActive,
                student.CreatedAt,
                student.UserId
            );
        }
    }
}
