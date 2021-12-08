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
using DNTPersianUtils.Core.IranCities;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdversController : Controller
    {
        private readonly DataContext _context;
        private readonly IAdverService _adverService;

        public AdversController(DataContext context, IAdverService adverService)
        {
            _context = context;
            _adverService = adverService;
        }
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var finalModel = Iran.Cities.ToList();
            finalModel = finalModel.OrderBy(x => x.ProvinceName).ThenBy(x => x.CityName).ToList();
            ViewBag.Cities = finalModel.ToList();
            ViewBag.pageSize = _adverService.GetAllAdverForAdmin(1, 1).Result.PageCount;
            //return Json(_adverService.GetAllAdverForAdmin(1, 10000).Result.ListOfData.Select(x => x.City));
            return View();
        }


    }
}
