using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace test_api.Filters
{
    public class InvalidOperationExceptionFilter : IExceptionFilter
    {
        public InvalidOperationExceptionFilter()
        {
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                var defaultBackgroundColor = Console.BackgroundColor;
                var defaultForegroundColor = Console.ForegroundColor;

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Error.Write("ERROR [Console.Error.Write]");

                Console.BackgroundColor = defaultBackgroundColor;
                Console.ForegroundColor = defaultForegroundColor;

                Console.Error.WriteLine($": {this.GetType()}");
                Console.Error.WriteLine($"{context.Exception.GetType()}: {context.Exception.Message}");
                Console.Error.WriteLine(context.Exception.StackTrace);
            }

            if (context.Exception is InvalidOperationException)
            {
                var data = new Dictionary<string, object>
                {
                    { "message", context.Exception.Message }
                };
                context.Result = new JsonResult(data)
                {
                    StatusCode = 400,
                };
            }
        }
    }
}
