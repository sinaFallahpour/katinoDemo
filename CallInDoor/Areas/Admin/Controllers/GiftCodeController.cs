using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using Service;
using Hangfire.Annotations;
using Domain.Utilities;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiftCodeController : Controller
    {
        private readonly DataContext _context;

        public GiftCodeController(DataContext context)
        {
            _context = context;
        }
        [HttpGet()]
        public async  Task<IActionResult> Index()
        {
            ViewBag.UserList = await _context.Users.Where(x => x.IsActive&&x.Role!=PublicHelper.ADMINROLE).Select(x => new UserList{Id= x.Id,Fullname= x.Fullname }).ToListAsync();
            return View();
        }
        public class UserList
        {
            public string Id { get; set; }
            public string Fullname { get; set; }
        }
    }
}
