using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ServicesController(DataContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostingEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var data = _context.Services.Include(x => x.Children).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services.Where(x =>x.ParentId == 0).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm] Services service)
        {
            if(service.ParentId != 0)
            {
                var parent = _context.Services.FirstOrDefault(x => x.Id == service.ParentId);
                if(parent.ParentId != 0)
                {
                    return new JsonResult(new { statusCode = "0", message = "خطا" });

                }

            }
            service.Date = DateTime.Now;
            _context.Services.Add(service);
            _context.SaveChanges();
            if (service.ParentId != 0)
            {
                var parent = _context.Services.Include(x =>x.Children).FirstOrDefault(x => x.Id == service.ParentId);
                parent.Children.Add(service);
                _context.SaveChanges();
            }
            return new JsonResult(new { statusCode = "200" , message = "انجام شد" });
        }
        public IActionResult Edit(int id)
        {
            var item = _context.Services.FirstOrDefault(x => x.Id == id);
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(Services services)
        {
            var item = _context.Services.FirstOrDefault(x => x.Id == services.Id);
            item.Description = services.Description;
            item.Title = services.Title;
            _context.SaveChanges();
            return new JsonResult(new { statusCode = "200", message = "انجام شد" });
        }
        public IActionResult Delete(int id)
        {
            var item = _context.Services.Include(x => x.Children).FirstOrDefault(x => x.Id == id);
            if (item.Children.Any())
                return new JsonResult(new { status ='0' , message = "خطا" });

            _context.Services.Remove(item);
            _context.SaveChanges();
            return new JsonResult(new { status = '1', message = "با موفقیت انجام شد" });

        }
        public IActionResult UploadImg(IFormFile upload)
        {
            if (upload.ContentType.Length <= 0 || upload.ContentType.Length > 300)
            {
                return Json(new { status = "0", message = " .حجم عکس باید کمتر از 300 کیلوبایت باشد" });
            }
            else
            {
                var fileName = DateTime.Now.Ticks.ToString();
                fileName += Path.GetFileName(upload.FileName);
                var path = _hostingEnvironment.WebRootPath + "/uploads/" + fileName;
                upload.CopyTo(new FileStream(path, FileMode.Create));
                return Json(new { fileName = fileName, uploaded = "1", url = "https://panel.katinojob.ir/uploads/" + fileName });
            }
        }
    }
}
