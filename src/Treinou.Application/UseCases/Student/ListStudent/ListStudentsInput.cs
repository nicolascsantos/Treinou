using MediatR;
using Treinou.Application.Common;
using Treinou.Domain.SeedWork.SearchableRepository;

namespace Treinou.Application.UseCases.Student.ListStudent
{
    public class ListStudentsInput : PaginatedListInput, IRequest<ListStudentsOutput>
    {
        public ListStudentsInput(
            int page = 1,
            int perPage = 15,
            string search = "",
            string sort = "",
            SearchOrder searchOrder = SearchOrder.ASCENDING
        ) : base(page, perPage, search, sort, searchOrder)
        {}

        public ListStudentsInput()
           : base(1, 15, "", "", SearchOrder.ASCENDING)
        { }        
    }
}
