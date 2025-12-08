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

        public Guid TeacherId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public StudentModelOutput(
            Guid id,
            string name,
            string email,
            string cpf,
            string phoneNumber,
            DateTime birthDate,
            double weight,
            double height,
            Guid teacherId,
            bool isActive,
            DateTime createdAt
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
            TeacherId = teacherId;
            IsActive = isActive;
            CreatedAt = createdAt;
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
                student.TeacherId,
                student.IsActive,
                student.CreatedAt
            );
        }
    }
}
