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
    public class AnswerQuestionsController : Controller
    {
        private readonly DataContext _context;

        public AnswerQuestionsController(DataContext context)
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
