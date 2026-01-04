
using Treinou.Domain.Repository;

namespace Treinou.Application.UseCases.Teacher.ListTeachers
{
    public class ListTeachers : IListTeachers
    {
        private readonly ITeacherRepository _teacherRepository;

        public ListTeachers(ITeacherRepository teacherRepository)
            => _teacherRepository = teacherRepository;


        public async Task<ListTeachersOutput> Handle(ListTeachersInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _teacherRepository.Search(request.ToSearchInput(), cancellationToken);

            var output = ListTeachersOutput.FromSearchOutput(searchOutput);

            return output;
        }
    }
}
