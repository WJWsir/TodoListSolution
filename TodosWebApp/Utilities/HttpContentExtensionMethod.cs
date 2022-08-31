using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TodosWebApp.Utilities
{
    public static class HttpContentExtensionMethod
    {
        public static string GetClientIP(this HttpContext context)
        
        {
            string result = context.Request.Headers["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = context.Request.Headers["REMOTE_ADDR"];
            }
            return result;
        }
    }
}
