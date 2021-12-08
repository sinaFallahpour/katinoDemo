using DNTPersianUtils.Core;
using Domain;
using Domain.DTO.Response;
using Domain.Utilities;
using Katino.Areas.Refrence.ViewModel;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Controllers
{

    [Route("api/[controller]")]
    public class RefrenceRequestController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IlogService _ilog;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RefrenceRequestController(DataContext context, IAccountService accountService,
                               IHttpContextAccessor httpContextAccessor, IlogService ilog, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _ilog = ilog;
            _webHostEnvironment = webHostEnvironment;
        }






        [ClaimsAuthorize]
        [HttpGet("GetAllRefrenceRequest")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        public async Task<IActionResult> GetAllRefrenceRequest()
        {
            try
            {
                //var user = await _accountService.GetCurrectUser();
                var refrencewsRequests = await _context.RefrenceDepositRequest
                                                 .Include(c => c.Refrence)
                                                 .Select(c => new
                                                 {
                                                     c.Id,
                                                     c.Amount,
                                                     c.RefrenceDescription,
                                                     c.AdminDescriptions,
                                                     RefrenceTransationStatus = c.RefrenceTransationStatus,
                                                     EnumDescription = c.RefrenceTransationStatus.GetEnumDescription(),
                                                     //CreateDate = c.CreateDate.ToPersianDateTextify(true).ToString(),
                                                     c.CreateDate,
                                                     c.Refrence.UserName,
                                                     c.Refrence.PhoneNumber,
                                                     c.Refrence.StaticPhoneNumber,
                                                     c.Refrence.ShebaNumber,
                                                     c.Refrence.Fullname
                                                 })
                                                 .OrderBy(c => c.RefrenceTransationStatus).ThenByDescending(c => c.CreateDate).ToListAsync();
                return new JsonResult(refrencewsRequests);
            }
            catch (Exception ex)
            {
                return new JsonResult("");
            }

        }





        [ClaimsAuthorize]
        [HttpPost("AcceptRequest")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<IActionResult> AcceptRequest([FromForm] RefrenceRequestConfirmation model)
        {
            var message = new List<string>();
            var requestFromDB = await _context.RefrenceDepositRequest.Where(c => c.Id == model.Id).FirstOrDefaultAsync();

            if (requestFromDB == null)
            {
                message.Add("یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.An_unhandled_error_occurred, message, false, null));
            }

            var userFromDB = await _context.Users.Where(c => c.Id == requestFromDB.RefrenceId).FirstOrDefaultAsync();
            if (userFromDB == null)
            {
                message.Add("یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.An_unhandled_error_occurred, message, false, null));
            }

            requestFromDB.RefrenceTransationStatus = RefrenceTransationStatus.Accepted;
            requestFromDB.AdminDescriptions = model.AdminDescription;

            userFromDB.RefrenceTotalPrice += requestFromDB.Amount;

            await _context.SaveChangesAsync();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }



        [ClaimsAuthorize]
        [HttpPost("RejectRequest")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<IActionResult> RejectRequest([FromForm] RefrenceRequestConfirmation model)
        {
            var message = new List<string>();
            var requestFromDB = await _context.RefrenceDepositRequest.Where(c => c.Id == model.Id).FirstOrDefaultAsync();

            if (requestFromDB == null)
            {
                message.Add("یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.An_unhandled_error_occurred, message, false, null));
            }

            var userFromDB = await _context.Users.Where(c => c.Id == requestFromDB.RefrenceId).FirstOrDefaultAsync();
            if (userFromDB == null)
            {
                message.Add("یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.An_unhandled_error_occurred, message, false, null));
            }
            requestFromDB.RefrenceTransationStatus = RefrenceTransationStatus.Rejected;
            requestFromDB.AdminDescriptions = model.AdminDescription;
            //userFromDB.RefrenceTotalPrice -= requestFromDB.Amount;


            await _context.SaveChangesAsync();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }





    }
}
