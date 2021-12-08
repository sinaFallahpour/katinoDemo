using Domain;
using Domain.DTO.ReportAdverts;
using Domain.DTO.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DNTPersianUtils.Core;
using Katino.Config.Extentions;
using Domain.Utilities;

namespace Katino.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class ReportAdvertSController : Controller
    {
        private readonly DataContext _context;
        private readonly IAccountService _accountService;
        private readonly IAdverService _adverService;
        public ReportAdvertSController(DataContext dataContext, IAccountService accountService, IAdverService adverService)
        {
            _context = dataContext;
            _accountService = accountService;
            _adverService = adverService;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("Create")]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<IActionResult> Create([Microsoft.AspNetCore.Mvc.FromBody] CreateReportAdvert input)
        {
            var message = new List<string>();

            if (input.AdvertId == 0)
            {
                message.Add(" آگهی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Values)
                {
                    foreach (var err in item.Errors)
                    {
                        message.Add(err.ErrorMessage);
                    }
                }
                message.Reverse();
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            var advert = await _adverService.GetAdverById(input.AdvertId);
            if (advert == null)
            {
                message.Add(" آگهی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            var user = await _accountService.GetCurrectUser();

            var reportAdvert = new ReportAdvert
            {
                User = user,
                UserId = user.Id,
                Date = DateTime.Now,
                Description = input.Description,
                JobAdvertisement = advert,
                JobAdvertisementId = advert.Id,
                Status = ReportAdvertStatus.Saved,
                Title = input.Title,
                Name = user.Fullname,
                Type = input.Type,
                PhoneNumber = user.UserName
            };
            await _context.ReportAdverts.AddAsync(reportAdvert);
            await _context.SaveChangesAsync();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("List")]
        [AllowAnonymous]
        public IActionResult List(int advertId)
        {
            var message = new List<string>();
            message.Add("ok");
            var data = _context.ReportAdverts.Where(x => x.JobAdvertisementId == advertId && x.Status == ReportAdvertStatus.Saved).Select(x => new
            {
                x.Description,
                date = x.Date.ToPersianDateTextify(false),
                x.Title,
                x.JobAdvertisementId,
                x.Name
            }).ToList();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, data));
        }
    }
}
