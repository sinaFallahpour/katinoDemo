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

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobSkillController : Controller
    {
        private readonly DataContext _context;
        private readonly ICategoryService _categoryService;

        public JobSkillController(DataContext context,ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }
        [HttpGet()]
        public async  Task<IActionResult> Index()
        {
            ViewBag.categories =  _categoryService.GetAllCategories();
            return View();
        }


    }
}
