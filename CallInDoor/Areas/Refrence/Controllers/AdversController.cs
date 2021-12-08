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
using Service.Interfaces.Account;
using Katino.Config.Extentions;

namespace Katino.Areas.Refrence.Controllers
{
    [Area("Refrence")]
    public class AdversController : Controller
    {
        private readonly DataContext _context;
        private readonly IAdverService _adverService;
        private readonly IAccountService _accountService;

        public AdversController(DataContext context, IAdverService adverService, IAccountService accountService)
        {
            _context = context;
            _adverService = adverService;
            _accountService = accountService;
        }

        //[ClaimsAuthorize]
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            //var user = _accountService.GetCurrectUser();
            //return Json(user);
            ViewBag.pageSize = _adverService.GetAllAdverForAdmin(1, 1).Result.PageCount;
            return View();
        }


    }
}
