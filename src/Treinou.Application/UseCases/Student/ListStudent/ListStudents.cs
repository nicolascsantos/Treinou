using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Student.ListStudent
{
    public class ListStudents : IListStudents
    {
        private readonly IStudentRepository _studentRepository;

        public ListStudents(IStudentRepository studentRepository)
            => _studentRepository = studentRepository;


        public async Task<ListStudentsOutput> Handle(ListStudentsInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _studentRepository.Search(request.ToSearchInput(), cancellationToken);

            var output = ListStudentsOutput.FromSearchOutput(searchOutput);

            return output;
        }
    }
}
