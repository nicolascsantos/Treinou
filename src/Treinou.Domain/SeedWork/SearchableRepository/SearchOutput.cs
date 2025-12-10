namespace Treinou.Domain.SeedWork.SearchableRepository
{
    public class SearchOutput<TAggregate> where TAggregate : class
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public int Total { get; set; }

        public IReadOnlyList<TAggregate> Items { get; set; }

        public SearchOutput(
            int page,
            int perPage,
            int total,
            IReadOnlyList<TAggregate> items
        )
        {
            Page = page;
            PerPage = perPage;
            Total = total;
            Items = items;
        }
    }
}
