using Treinou.Domain.Entities;

namespace Treinou.Application.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string CPF { get; private set; }

        public string PhoneNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Guid? TeacherId { get; private set; }

        public string? UserId { get; private set; }

        public StudentDTO(
            Guid id,
            string name,
            string email,
            string cpf,
            string phoneNumber,
            DateTime birthDate,
            double weight,
            double height,
            bool isActive,
            DateTime createdAt,
            Guid? teacherId,
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
            IsActive = isActive;
            CreatedAt = createdAt;
            TeacherId = teacherId;
            UserId = userId;
        }

        public static StudentDTO FromStudent(Student student)
            => new StudentDTO(
                student.Id,
                student.Name,
                student.Email.Address,
                student.CPF.Number,
                student.PhoneNumber.Number,
                student.BirthDate,
                student.Weight,
                student.Height,
                student.IsActive,
                student.CreatedAt,
                student.TeacherId,
                student.UserId
            );
    }
}
