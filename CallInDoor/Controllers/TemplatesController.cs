using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Domain.DTO.Response;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly DataContext _context;
        public TemplatesController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("List")]
        [AllowAnonymous]
        public IActionResult List()
        {
            var data = _context.ResomeColors.ToList();
            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, data));
        }
    }
}
