using Treinou.Application.Common;
using Treinou.Application.UseCases.Teacher.Common;
using Treinou.Domain.SeedWork.SearchableRepository;
using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.Teacher.ListTeachers
{
    public class ListTeachersOutput : PaginatedListOutput<TeacherModelOutput>
    {
        public ListTeachersOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<TeacherModelOutput> items
        ) : base(page, perPage, total, items)
        {
        }

        public static ListTeachersOutput FromSearchOutput(SearchOutput<Entity.Teacher> searchOutput)
        => new ListTeachersOutput(
                searchOutput.Page,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items.Select(TeacherModelOutput.FromTeacher).ToList()
        );
    }
}
