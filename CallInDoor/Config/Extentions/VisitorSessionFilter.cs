using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Config.Extentions
{
    public class VisitorSessionFilter : IActionFilter
    {
        private readonly DataContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public VisitorSessionFilter(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // session check
            var visitorId = context.HttpContext.Session.GetString("visitorid");
            if (string.IsNullOrWhiteSpace(visitorId))
            {
                context.HttpContext.Session.SetString("visitorid", Guid.NewGuid().ToString());

                var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var browser = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();

                if (ip != "::1")
                {
                    var visitor = new SiteVisit
                    {
                        Browser = browser,
                        IP = ip
                    };

                    _context.SiteVisits.Add(visitor);

                    _context.SaveChanges();
                }
            }
            // end session check
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
