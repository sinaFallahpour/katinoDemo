
//using Domain;
//using Domain.DTO.Response;
//using Domain.Utilities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using System.Web.Http.Filters;

//namespace Katino
//{
//    public class CheckTokenIsValidAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
//    {
       
//        public void OnActionExecuting(ActionExecutingContext context, IHttpContextAccessor httpContextAccessor, DataContext dataContext)
//        {
//            var currentSerialNumber = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == PublicHelper.SerialNumberClaim)?.Value;
//            var username = _dataContext.Users.Where(x => x.SerialNumber == currentSerialNumber)
//               .Select(c => c.UserName)
//               .FirstOrDefault();
//            var err = new List<string>();
//            err.Add("توکن نا معتبر");
//            //if (string.IsNullOrEmpty(username))
//                context.Result = new UnauthorizedObjectResult(new ResponseResult(StatusCode.unAuthorize, err, false, null));
//        }
//    }
//}
