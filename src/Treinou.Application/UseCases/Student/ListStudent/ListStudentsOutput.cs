using Treinou.Application.Common;
using Treinou.Application.UseCases.Student.Common;
using Treinou.Domain.SeedWork.SearchableRepository;
using Entity = Treinou.Domain.Entities;

namespace Treinou.Application.UseCases.Student.ListStudent
{
    public class ListStudentsOutput : PaginatedListOutput<StudentModelOutput>
    {
        public ListStudentsOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<StudentModelOutput> items
        ) : base(page, perPage, total, items)
        {
            
        } 

        public static ListStudentsOutput FromSearchOutput(SearchOutput<Entity.Student> searchOutput)
        =>  new ListStudentsOutput(
                searchOutput.Page,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items.Select(StudentModelOutput.FromStudent).ToList()
        );
        
    }
}
