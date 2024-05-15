using System.Text.Json;

namespace VFoody.API.MIddleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        private record ApiError(string PropertyName, string ErrorMessage);
    }
}
