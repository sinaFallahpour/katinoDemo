using Domain;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Katino.Areas.Refrence.Controllers
{
    [Area("Refrence")]
    //[Authorize(Roles = PublicHelper.RefrenceRole)]

    public class ProfileController : Controller
    {
        private readonly DataContext _context;
        private readonly IAccountService _accountService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfileController(DataContext context, IAccountService accountService, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _accountService = accountService;
            _hostingEnvironment = hostingEnvironment;
        }

        private string upload(IFormFile image)
        {
            var fileName = DateTime.Now.Ticks.ToString();
            fileName += Path.GetFileName(image.FileName);
            var path = _hostingEnvironment.WebRootPath + "/Img/Gallery" + fileName;
            image.CopyTo(new FileStream(path, FileMode.Create));
            return fileName;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
