using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunaria.AspNetCore.Filters.ExceptionLogging
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an exception logging service as a scoped instance.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Defines ExceptionLoggingFilterOptions for this instance.</param>
        /// <returns>Continues the IServiceCollection chain.</returns>
        public static IServiceCollection AddExceptionLogging(this IServiceCollection services, ExceptionLoggingFilterOptions? options = null)
        {
            options ??= new ExceptionLoggingFilterOptions();
            services.AddExceptionLogging(o =>
            {
                o = options;
            });
            return services;
        }

        /// <summary>
        /// Adds an exception logging service as a scoped instance.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">Defines ExceptionLoggingFilterOptions for this instance.</param>
        /// <returns>Continues the IServiceCollection chain.</returns>
        public static IServiceCollection AddExceptionLogging(this IServiceCollection services, Action<ExceptionLoggingFilterOptions> configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            services.TryAddScoped<ExceptionLoggingFilter>();
            services.Configure(configuration);
            return services;
        }
    }
}
