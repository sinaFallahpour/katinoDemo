using DNTPersianUtils.Core;
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

    public class AssignedResomesController : Controller
    {
        private readonly DataContext _context;
        public AssignedResomesController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int adverId)
        {
            var adver = await _context.JobAdvertisements.FindAsync(adverId);

            if (adver == null)
            {
                return Json(null);
            }
            var asignResome = await _context.AsignResomes
                 .Where(x => x.JobAdvertisementId == adverId)
                 .Include(x => x.JobAdvertisement).ThenInclude(x => x.Company)
                .Include(x => x.Resome).ThenInclude(x=> x.Employee)
                .OrderByDescending(x => x.Resome.Employee)
                .Select(x => new GetAllResomesInfoForAdverForAdmin()
                {
                    AsignResomeId = x.Id,
                    JobAdvertisement = x.JobAdvertisement,
                    Id = adverId,
                    Company = x.JobAdvertisement.Company,
                    Employee = x.Resome.Employee,
                    Resome = x.Resome,
                    AsingResomeStatus = x.AsingResomeStatus,
                    Date = x.CreateDate.ToShortPersianDateTimeString(true),
                    FullName = x.Resome.Employee.Fullname,
                    HasComment = (x.CommentAsignResomes.Count != 0) ? true : false,
                    IsMark = (x.MarkAsignResomes.Count != 0) ? true : false,
                    JobTitle = (x.Resome.UserJobShortDescriptions.FirstOrDefault() != null) ?
                    x.Resome.UserJobShortDescriptions.FirstOrDefault().JobTitle : "",
                    UserAvatar = x.Resome.Employee.Logo,
                    City = x.Resome.Employee.City,
                    Gender = x.Resome.Employee.Gender,
                    Senioritylevel = x.Resome.UserJobPreferences.Senioritylevel,
                    ResomeId = x.ResomeId,
                    SpecialEmpolyee = x.Resome.Employee.SpecialEmpolyee

                })
                .OrderByDescending(x => x.SpecialEmpolyee)
                .ToListAsync();

            return View(asignResome);
            return Json(asignResome);
        }
    }
}
