using Domain;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public GalleryController(DataContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostingEnvironment = webHostEnvironment;
        }
        [HttpGet("GetCompanyImages")]
        public async Task<ActionResult> GetCompanyImages(string id)
        {
            var company = _context.Users.Include(x => x.Gallery).FirstOrDefault(x => x.Id == id);
            return new JsonResult(company.Gallery.Select(x => x.Url).ToList());
        }
        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var host = _hostingEnvironment.WebRootPath;
                var savePath = Path.Combine(host, "Img/Gallery");
                var uploadResult = FileUploader.UploadImage(image, savePath, compression: 70, width: 512, height: 512);
                if (!uploadResult.succsseded)
                {
                    return new JsonResult("1");
                }
                else
                {
                    var img = uploadResult.result;
                    return new JsonResult("/Img/Gallery/" + img);
                }
            }
            else
            {
                return new JsonResult("2");
            }
        }
        [Route("AddImgToGallery")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AddImgToGallery([FromForm]string id, [FromForm] string image)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            var img = new Image();
            img.Date = DateTime.Now;
            img.Url = image;
            img.UserId = user.Id;
            await _context.Images.AddAsync(img);
            await _context.SaveChangesAsync();
            return new JsonResult(image);

        }
        [Route("DeleteImgFromGallery")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeleteImgFromGallery([FromForm] string id, [FromForm] string url)
        {
            var company = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            var images =  _context.Images.FirstOrDefault(x => x.Url == url);
            _context.Remove(images);
            await _context.SaveChangesAsync();
            return new JsonResult(true);
        }
    }
}
