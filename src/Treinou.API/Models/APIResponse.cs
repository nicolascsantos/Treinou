namespace Treinou.API.Models
{
    public class APIResponse<TData> where TData : class
    {
        public TData Data { get; set; }

        public APIResponse(TData data)
            => Data = data;
    }
}
