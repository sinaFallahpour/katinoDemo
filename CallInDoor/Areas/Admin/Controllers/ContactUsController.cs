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
using DNTPersianUtils.Core;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactUsController : Controller
    {
        private readonly DataContext _context;


        public ContactUsController(DataContext context)
        {
            _context = context;
        }
        [Route("Admin/ContactUsManager")]
        [HttpGet()]
        public async Task<IActionResult> index()
        {
            return View();

        }



    }
}
