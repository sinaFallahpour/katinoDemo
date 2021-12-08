

using Domain;
using Domain.DTO.Response;
using Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Config.Extentions
{
    public class RefrenceAuthorizeAttribute : Attribute, IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext context) { }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var errors = new List<string>();
            //errors.Add("کابر نامعتبر");



            var _dataContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();

            var userSerialNumber = context.HttpContext.User?.Claims?
                .FirstOrDefault(x => x.Type == PublicHelper.SerialNumberClaim)?.Value;
            userSerialNumber = userSerialNumber ?? "notvalid:)";
            var user = _dataContext.Users
           .FirstOrDefault(x => x.SerialNumber == userSerialNumber);
            context.Result = new OkObjectResult(JsonConvert.SerializeObject(user));

            //if (user == null)
            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //    {
            //        //Area = "Refrence",
            //        action = "login",
            //        controller = "Refrence"
            //    }));

            //var userSerialNumberInDataBase = user?.SerialNumber;
            //if (userSerialNumber != userSerialNumberInDataBase
            //    || !user.IsActive || user.Role != PublicHelper.RefrenceRole)

            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //    {
            //        //Area = "Refrence",
            //        action = "login",
            //        controller = "Refrence"
            //    }));

        }


    }
}
//
