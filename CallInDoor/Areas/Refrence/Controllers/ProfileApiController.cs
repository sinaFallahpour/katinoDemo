using Domain;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Areas.Refrence.Controllers
{

    [Route("api/[controller]")]
    //[Authorize(Roles = PublicHelper.ADMINROLE)]
    //[ClaimsAuthorize]



    //[Route("api/[controller]")]
    //[ApiController]
    ////[Authorize(Roles = PublicHelper.RefrenceRole)]
    public class ProfileApiController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAccountService _accountService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfileApiController(DataContext context, IAccountService accountService, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _accountService = accountService;
            _hostingEnvironment = hostingEnvironment;
        }
        [NonAction]
        private string upload(IFormFile image)
        {
            var fileName = DateTime.Now.Ticks.ToString();
            fileName += Path.GetFileName(image.FileName);
            var path = _hostingEnvironment.WebRootPath + "/Img/Gallery" + fileName;
            image.CopyTo(new FileStream(path, FileMode.Create));
            return fileName;
        }
        [HttpGet("Info")]
        public async Task<IActionResult> Info()
        {
            var user = await _accountService.GetCurrectUser();
            return new JsonResult(user);
       
        
        
        }
        [HttpPost("Submit")]
        public async Task<IActionResult> Submit(string iframe, string description)
        {
            var user = await _accountService.GetCurrectUser();
            user.Iframe = iframe;
            user.Description = description;
            _context.SaveChanges();
            return new JsonResult(new { iframe, description });

        }
        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage(IFormFile image)
        {
            if (image == null)
            {
                return new JsonResult(new { status = '0', message = "عکس را خالی نگذارید" });
            }
            if (Path.GetExtension(Path.GetFileName(image.FileName)) != ".jpg"
                && Path.GetExtension(Path.GetFileName(image.FileName)) != ".png"
                && Path.GetExtension(Path.GetFileName(image.FileName)) != ".jpeg"
                )
            {
                return new JsonResult(new { status = '0', message = "فرمت اشتباه" });
            }
            var user = _context.Users.FirstOrDefault(x => x.UserName == "09118908255");
            var img = new Domain.Entities.Image
            {
                Date = DateTime.Now,
                Url = "/Img/Gallery" + upload(image),
                UserId = user.Id
            };
            _context.Images.Add(img);
            await _context.SaveChangesAsync();
            return new JsonResult(new { status = '0', message = "انجام شد" });
        }
        [HttpGet("DeleteImage")]
        public IActionResult DeleteImage(int id)
        {
            var image = _context.Images.FirstOrDefault(x => x.Id == id);
            _context.Images.Remove(image);
            _context.SaveChanges();
            return new RedirectResult("/Refrence/profile");
        }
    }
}
