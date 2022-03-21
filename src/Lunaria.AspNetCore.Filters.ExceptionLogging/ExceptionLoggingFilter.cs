using Lunaria.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lunaria.AspNetCore.Filters.ExceptionLogging
{
    public class ExceptionLoggingFilter : IExceptionFilter
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ExceptionLoggingFilterOptions _options;

        public ExceptionLoggingFilter(ILoggerFactory loggerFactory, ExceptionLoggingFilterOptions options)
        {
            _loggerFactory = loggerFactory;
            _options = options;
        }

        public ExceptionLoggingFilter(ILoggerFactory loggerFactory, IOptions<ExceptionLoggingFilterOptions> options = null)
            : this(loggerFactory, options?.Value ?? new ExceptionLoggingFilterOptions())
        {
        }

        public void OnException(ExceptionContext context)
        {
            string loggerCategory = context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor
                ? controllerActionDescriptor.ControllerTypeInfo.FullName
                    ?? controllerActionDescriptor.ControllerTypeInfo.Name
                : context.ActionDescriptor.DisplayName;
            var logger = _loggerFactory.CreateLogger(loggerCategory);
            if (!_options.ExceptionMapper.TryGetMapping(context.Exception, out var logAction)) return;
            logAction?.Invoke(context.HttpContext, context.Exception, logger);
        }
    }
}
