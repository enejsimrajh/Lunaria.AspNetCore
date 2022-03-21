using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunaria.AspNetCore.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string ToShortDescriptionString(this HttpRequest httpRequest)
        {
            return $"{httpRequest.Method} {httpRequest.Path}";
        }
    }
}
