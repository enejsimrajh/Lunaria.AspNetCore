using Lunaria.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Lunaria.AspNetCore.Filters.ExceptionLogging
{
    internal class ExceptionLogger
    {
        private readonly ILogger _logger;
        private readonly HttpContext _httpContext;
        private readonly ExceptionLoggingFilterOptions _options;

        internal ExceptionLogger(ILogger logger, HttpContext context, ExceptionLoggingFilterOptions options)
        {
            _logger = logger;
            _httpContext = context;
            _options = options;
        }

        internal void Log(LogLevel level, Exception exception)
        {
            if (_options.PrintEndpoint)
            {
                string endpoint = _httpContext.Request.ToShortDescriptionString();
                _logger.Log(level, exception, "An exception has occurred while executing the request. Endpoint: {Endpoint}", endpoint);
            }
            else
            {
                _logger.Log(level, exception, "An exception has occurred while executing the request.");
            }
        }
    }
}
