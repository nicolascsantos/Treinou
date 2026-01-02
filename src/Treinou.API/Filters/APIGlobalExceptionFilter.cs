using Microsoft.AspNetCore.Mvc.Filters;

namespace Treinou.API.Filters
{
    public class APIGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
