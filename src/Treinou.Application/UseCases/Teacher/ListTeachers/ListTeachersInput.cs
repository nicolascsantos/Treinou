using MediatR;
using Treinou.Application.Common;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Application.UseCases.Teacher.ListTeachers
{
    public class ListTeachersInput : PaginatedListInput, IRequest<ListTeachersOutput>
    {
        public ListTeachersInput(
            int page,
            int perPage,
            string search,
            string sort,
            SearchOrder dir
        ) : base(page, perPage, search, sort, dir)
        {
        }
        public ListTeachersInput()
           : base(1, 15, "", "", SearchOrder.ASCENDING)
        { }
    }
}
