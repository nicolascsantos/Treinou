namespace Treinou.Domain.SeedWork.SearchableRepository
{
    public class SearchOutput<TAggregate> where TAggregate : class
    {
        public int Page { get; set; }

        public int CurrentPage { get; set; }

        public int Total { get; set; }

        public IReadOnlyList<TAggregate> Items { get; set; }

        public SearchOutput(
            int page,
            int currentPage,
            int total,
            IReadOnlyList<TAggregate> items
        )
        {
            Page = page;
            CurrentPage = currentPage;
            Total = total;
            Items = items;
        }
    }
}
