namespace Treinou.API.Models
{
    public class APIResponseListMeta
    {
        public int CurrentPage { get; set; }

        public int PerPage { get; set; }

        public int Total { get; set; }

        public APIResponseListMeta(int currentPage, int perPage, int total)
        {
            CurrentPage = currentPage;
            PerPage = perPage;
            Total = total;
        }
    }
}
