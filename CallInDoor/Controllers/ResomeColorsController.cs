using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Katino.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class ResomeColorsController : Controller
    {
        private readonly DataContext _context;
        private readonly IAccountService _accountService;
        private readonly IAdverService _adverService;
        public ResomeColorsController(DataContext dataContext, IAccountService accountService, IAdverService adverService)
        {
            _context = dataContext;
            _accountService = accountService;
            _adverService = adverService;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("List")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public IActionResult List()
        {
            var message = new List<string>();
            message.Add("ok");
            var data = _context.ResomeColors.ToList();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, data));
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("Create")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public IActionResult Create([FromForm]string text, [FromForm] string bg)
        {
            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            var ResomeColor = new ResomeColors
            {
                Background = bg,
                Date = DateTime.Now,
                Text = text
            };
            _context.ResomeColors.AddAsync(ResomeColor);
            _context.SaveChanges();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("Delete")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public IActionResult Delete(int id)
        {
            var message = new List<string>();
            if(id == 1)
            {
                message.Add("Error");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, true, true));
            }
            var item = _context.ResomeColors.FirstOrDefault(x => x.Id == id);

            _context.ResomeColors.Remove(item);
            _context.SaveChanges();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }
    }
}
