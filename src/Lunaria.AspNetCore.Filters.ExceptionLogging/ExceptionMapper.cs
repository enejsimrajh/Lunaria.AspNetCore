using Lunaria.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunaria.AspNetCore.Filters.ExceptionLogging
{
    internal class ExceptionMapper
    {
        private readonly Dictionary<Type, Action<HttpContext, Exception, ILogger>> _mappings = new();
        private readonly ExceptionLoggingFilterOptions _options;

        public ExceptionMapper(ExceptionLoggingFilterOptions options)
        {
            _options = options;
        }

        public void Map<T>(LogLevel logLevel) where T : Exception
        {
            Map<T>((context, exception, logger) =>
            {
                var exceptionLogger = new ExceptionLogger(logger, context, _options);
                exceptionLogger.Log(logLevel, exception);
            });
        }

        internal void Map<T>(Action<HttpContext, T, ILogger> action) where T : Exception
        {
            _mappings[typeof(T)] = (HttpContext context, Exception exception, ILogger logger) => action(context, (T)exception, logger);
        }

        internal bool TryGetMapping(Exception exception, out Action<HttpContext, Exception, ILogger>? mapping)
        {
            var exceptionType = exception.GetType();
            if (_mappings.TryGetValue(exceptionType, out mapping)) return true;

            KeyValuePair<Type, Action<HttpContext, Exception, ILogger>>? mappingCandidate = null;
            foreach (var mappingEntry in _mappings)
            {
                if (exceptionType.IsSubclassOf(mappingEntry.Key))
                {
                    if (mappingCandidate is null)
                    {
                        mappingCandidate = mappingEntry;
                    }
                    else
                    {
                        if (mappingEntry.Key.IsSubclassOf(mappingCandidate.Value.Key))
                        {
                            mappingCandidate = mappingEntry;
                        }
                    }
                }
            }

            mapping = mappingCandidate?.Value;
            return mapping is not null;
        }
    }
}
