using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Student.ListStudent
{
    public class ListStudents : IListStudents
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;

        public ListStudents(IStudentRepository studentRepository, ITeacherRepository teacherRepository)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task<ListStudentsOutput> Handle(ListStudentsInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _studentRepository.Search(request.ToSearchInput(), cancellationToken);

            foreach (var student in searchOutput.Items)
            {
                if (student.TeacherId != default)
                {
                    var teacher = await _teacherRepository.Get(student.TeacherId, cancellationToken);
                    student.Teacher = teacher;
                }
            }

            var output = ListStudentsOutput.FromSearchOutput(searchOutput);

            return output;
        }
    }
}
