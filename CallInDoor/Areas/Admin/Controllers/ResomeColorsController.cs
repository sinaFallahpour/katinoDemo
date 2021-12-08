using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResomeColorsController : Controller
    {
        private readonly DataContext _context;
        public ResomeColorsController(DataContext dataContext)
        {
            _context = dataContext;
        }
        public IActionResult Index()
        {
            //var adver = _context.JobAdvertisements.Include(x => x.ResomeColor).FirstOrDefault(x => x.Id == 2240);
            //return Json(adver);
            ////var r = _context.ResomeColors.FirstOrDefault(x => x.Id == 1);
            ////adver.ResomeColor = r;
            ////adver.ResomeColorId = r.Id;
            ////_context.SaveChanges();
            var data = _context.ResomeColors.ToList();
            return View(data);
        }
    }
}
