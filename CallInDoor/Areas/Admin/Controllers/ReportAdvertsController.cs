using DNTPersianUtils.Core;
using Domain;
using Domain.DTO.Response;
using Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportAdvertsController : Controller
    {
        private readonly DataContext _context;
        private readonly IAccountService _accountService;
        private readonly IAdverService _adverService;
        public ReportAdvertsController(DataContext dataContext, IAccountService accountService, IAdverService adverService)
        {
            _context = dataContext;
            _accountService = accountService;
            _adverService = adverService;
        }
        public IActionResult Index(int id)
        {
            if (id == 0)
            {
                var data = _context.ReportAdverts.Where(x => x.Status == ReportAdvertStatus.Saved).Include(x => x.JobAdvertisement).ToList();
                foreach (var item in data)
                {
                    item.IsSeen = true;
                }
                _context.SaveChanges();
                return View(data);
            }
            else
            {
                var data = _context.ReportAdverts.Where(x => x.Status == ReportAdvertStatus.Saved && x.JobAdvertisementId == id).Include(x => x.JobAdvertisement).ToList();
                foreach (var item in data)
                {
                    item.IsSeen = true;
                }
                _context.SaveChanges();
                return View(data);
            }
        }
        public IActionResult List()
        {
            var message = new List<string>();
            message.Add("ok");
            var data = _context.ReportAdverts.Where(x => x.Status == ReportAdvertStatus.Saved).Include(x => x.JobAdvertisement).Select(x => new
            {
                x.Description,
                date = x.Date.ToPersianDateTextify(false),
                x.Title,
                x.JobAdvertisementId,
                JobAdvertisement = x.JobAdvertisement.Title,
                Type = x.Type.GetDisplayAttributeFrom()
            }).ToList();
            return Json(data);
        }
        public IActionResult Delete(int id)
        {
            var item = _context.ReportAdverts.FirstOrDefault(x => x.Id == id);
            item.Status = ReportAdvertStatus.Deleted;
            _context.SaveChanges();
            return Json(new { status = "1", message = "با موفقیت انجام شد" });
        }
    }
}
