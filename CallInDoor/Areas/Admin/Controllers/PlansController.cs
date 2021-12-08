using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlansController : Controller
    {
        private readonly DataContext _context;

        public PlansController(DataContext context)
        {
            _context = context;
        }
        [HttpGet()]
        public async  Task<IActionResult> Index()
        {
            return View();
        }


    }
}
