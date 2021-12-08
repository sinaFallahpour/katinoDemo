using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Katino.Models;
using Domain;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Katino.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Test()
        {
            var users = _context.Users.Where(x => x.Role == PublicHelper.EmployeeRole && !string.IsNullOrWhiteSpace(x.Email)).Include(x => x.Resome).ThenInclude(x => x.UserJobSkills).ThenInclude(x => x.JobSkill).ThenInclude(x => x.Category).ToList();
            var list = new List<CMCCCCC>();
            var ss = new List<List<int>>();

            foreach (var item in users)
            {
                var sadsd = item.Resome.UserJobSkills.Select(x => x.JobSkill.Category.Id).ToList();
                ss.Add(sadsd);
                var d =item.Resome.UserJobSkills.Select(x => new CMCCCCC
                {
                    C = x.JobSkill.Category.Name,
                    U = item.UserName,
                }).FirstOrDefault();
                list.Add(d);
            }
            return Json(ss);

        }
        public class CMCCCCC
        {
            public string U { get; set; }
            public string C { get; set; }
            public int ff { get; set; }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
