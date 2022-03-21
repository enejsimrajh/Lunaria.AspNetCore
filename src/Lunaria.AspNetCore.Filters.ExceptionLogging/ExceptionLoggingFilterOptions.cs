using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Lunaria.AspNetCore.Filters.ExceptionLogging
{
    public class ExceptionLoggingFilterOptions
    {
        internal ExceptionMapper ExceptionMapper { get; init; }

        public ExceptionLoggingFilterOptions()
        {
            ExceptionMapper = new(this);

            Map<Exception>(LogLevel.Error);
            Map<ArgumentException>(LogLevel.Warning);
            Map<KeyNotFoundException>(LogLevel.Warning);
        }

        public bool PrintEndpoint { get; set; } = true;

        public void Map<T>(LogLevel logLevel) where T : Exception => ExceptionMapper.Map<T>(logLevel);

        public void Map<T>(Action<HttpContext, T, ILogger> configure) where T : Exception => ExceptionMapper.Map(configure);
    }
}
