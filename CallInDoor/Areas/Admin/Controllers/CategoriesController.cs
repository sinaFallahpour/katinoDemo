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
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICategoryService _categoryService;

        public CategoriesController(DataContext context,ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }
        [HttpGet()]
        public async  Task<IActionResult> Index()
        {
            ViewBag.categories = _context.Categories.Where(x => x.IsActive && x.CategoryId == null).OrderBy(x => x.Name).Select(x => new ListOfCategoriesForSelect
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return View();
        }


    }
}
