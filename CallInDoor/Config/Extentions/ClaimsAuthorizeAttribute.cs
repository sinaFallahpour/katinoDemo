

using Domain;
using Domain.DTO.Response;
using Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Config.Extentions
{
    public class ClaimsAuthorizeAttribute : Attribute, IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext context) { }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var errors = new List<string>();
            errors.Add("کابر نامعتبر");



            var _dataContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();
           
            var userSerialNumber = context.HttpContext.User?.Claims?
                .FirstOrDefault(x => x.Type == PublicHelper.SerialNumberClaim)?.Value;
            userSerialNumber = userSerialNumber ?? "notvalid:)";
            var user = _dataContext.Users
           .FirstOrDefault(x => x.SerialNumber == userSerialNumber);
            if (user == null)
                context.Result = new UnauthorizedObjectResult(new ResponseResult(StatusCode.unAuthorize, errors, false, null));
            var userSerialNumberInDataBase = user?.SerialNumber;
            if (userSerialNumber != userSerialNumberInDataBase || !user.IsActive)
                context.Result = new UnauthorizedObjectResult(new ResponseResult(StatusCode.unAuthorize, errors, false, null));


        }


    }
}
//
