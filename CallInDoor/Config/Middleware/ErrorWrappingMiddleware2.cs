using Domain.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Config.Middleware
{
    public class ErrorWrappingMiddleware2
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorWrappingMiddleware2> _logger;

        public ErrorWrappingMiddleware2(RequestDelegate next, ILogger<ErrorWrappingMiddleware2> logger

            )
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context,IlogService _ilog)
        {
            var error = new List<string>();

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)

            {
                //_logger.LogError(EventIds.GlobalException, ex, ex.Message);
                context.Response.StatusCode = 500;
                await _ilog.CreateLog(ex.Message,ex.GetType().ToString(), "ErrorWrappingMiddleware2", "ErrorWrappingMiddleware2");
                error.Add(JsonConvert.SerializeObject(ex));

            }

            if (!context.Response.HasStarted)
            {
                if (context.Response.StatusCode == 500)
                {
                    context.Response.ContentType = "application/json";

                    var response = new ResponseResult(StatusCode.another, error, false, null);

                    var json = JsonConvert.SerializeObject(response);

                    await context.Response.WriteAsync(json);
                }
                else
                {
                    context.Response.ContentType = "application/json";

                    var response = new ResponseResult(context.Response.StatusCode);

                    var json = JsonConvert.SerializeObject(response);

                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}
