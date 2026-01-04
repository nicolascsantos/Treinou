using Treinou.Application.DTOs;

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

        public List<StudentDTO> Students { get; set; }

        public TeacherModelOutput(
            Guid id,
            string name,
            string email,
            string cpf,
            string cref,
            string description,
            string phoneNumber,
            DateTime birthDate,
            DateTime createdAt,
            List<StudentDTO> students
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
            Students = students;
        }

        public static TeacherModelOutput FromTeacher(Domain.Entities.Teacher teacher)
        {
            List<StudentDTO> students = new List<StudentDTO>();
            if (teacher.Students.Count > 0)
            {
                foreach (var student in teacher.Students)
                {
                    students.Add(new StudentDTO(
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
                        student.TeacherId
                    ));
                }
            }

            return new TeacherModelOutput(
                teacher.Id,
                teacher.Name,
                teacher.Email.Address,
                teacher.CPF.Number,
                teacher.CREF.Number,
                teacher.Description,
                teacher.PhoneNumber.Number,
                teacher.BirthDate,
                teacher.CreatedAt,
                students
                );
        }
    }
}
