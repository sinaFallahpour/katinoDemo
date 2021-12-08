using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using Hangfire.Annotations;
using Domain.Utilities;
using Domain.DTO.Response;
using DNTPersianUtils.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;
using Katino.Hubs;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Service.Interfaces.Account;
using Service.Interfaces.JwtManager;
using DNTPersianUtils.Core.IranCities;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Security.Claims;

namespace Katino.Areas.Refrence.Controllers
{
    [Area("Refrence")]
    public class RefrenceAccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IAccountService _accountService;
        private readonly IJwtManager _jwtManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RefrenceAccountController(DataContext context, IHubContext<ChatHub> hub, IAccountService accountService, IJwtManager jwtManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hub = hub;
            _accountService = accountService;
            _jwtManager = jwtManager;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet()]
        [Route("Refrence/Login")]
        public async Task<IActionResult> RefrenceLogin()
        {

            return View();
        }

        [HttpGet()]
        [Route("Refrence/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RefrenceRegister()
        {
            var cities = Iran.Cities.OrderBy(x => x.ProvinceName).ThenBy(x => x.CountyName).Select(
                x => x.ProvinceName + "-" + x.CountyName
                ).ToList();
            ViewBag.Cities = cities.Distinct().ToList();
            return View();
        }

        //[RefrenceAuthorize]
        //[Authorize(Roles = PublicHelper.RefrenceRole)]
        [HttpGet()]
        [Route("Refrence/Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }


        [HttpGet()]
        [Route("Refrence/Companies")]

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet()]
        [Route("Refrence/Users")]
        public async Task<IActionResult> Users()
        {
            return View();
        }

    }
}
