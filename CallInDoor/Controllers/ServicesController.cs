using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Katino.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    //[Authorize(Roles = PublicHelper.EmployeeRole)]
    //[ClaimsAuthorize]
    public class ServicesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAccountService _accountService;
        public ServicesController(DataContext dataContext, IAccountService accountService)
        {
            _context = dataContext;
            _accountService = accountService;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("List")]
        public IActionResult List()
        {
            var data = _context.Services.Include(x => x.Children).Select(x => new{
                x.Title,
                x.Id,
                x.ParentId,
                Children = x.Children.Select(y => new { 
                    y.Id,
                    y.Title
                })
            }).ToList();
            data = data.Where(x => x.ParentId == 0).ToList();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, new List<string>(),true,data));
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("Detail")]
        public IActionResult Detail(int id)
        {
            var data = _context.Services.FirstOrDefault(x => x.Id == id);
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, new List<string>(),true,data));
        }
    }
}
