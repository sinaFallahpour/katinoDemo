using Domain;
using Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNTPersianUtils;
using DNTPersianUtils.Core;
using Domain.DTO.Response;
using Katino.Config.Extentions;
using System.ComponentModel.DataAnnotations;
using Katino.Areas.Refrence.ViewModel;
using Domain.Entities;
using static Katino.Areas.Refrence.Controllers.PaymentController;

namespace Katino.Areas.Refrence.Controllers
{

    /// <summary>
    /// request of reference for getting  mony back
    /// </summary>
    [Area("Refrence")]
    //[Route("api/[area]/[controller]")]
    public class RefrenceRequestController : Controller
    {



        private readonly DataContext _context;
        private readonly IAccountService _accountService;

        public RefrenceRequestController(DataContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        
        
        
        
        
        [HttpGet]
        //[Authorize(Roles = PublicHelper.RefrenceRole)]
        public async Task<IActionResult> TotalPrice()
        {
            //var message = new List<string>();
            var user = await _accountService.GetCurrectUser();
            return Json(user?.RefrenceTotalPrice);
        }
        
        
        
        
        
        [HttpGet]
        //[Authorize(Roles = PublicHelper.RefrenceRole)]
        public async Task<IActionResult> Index()
        {
            var message = new List<string>();
            var user = await _accountService.GetCurrectUser();

            if (user == null)
            {
                ViewBag.AccountBalance = 0;
                return View();
            }
            ViewBag.AccountBalance = user.RefrenceTotalPrice;
            
            
            
            //ViewBag.AccountBalance = await _context.RefrenceDepositRequest
            //  .Where(c => c.RefrenceTransationStatus == RefrenceTransationStatus.Accepted && c.RefrenceId == user.Id)
            //                .SumAsync(c => c.Amount);

            return View();
        }




        [HttpGet()]
        [Authorize(Roles = PublicHelper.RefrenceRole)]
        public async Task<IActionResult> GetRefrencesRequests()
        {
            var user = await _accountService.GetCurrectUser();
            var refrenecFromDB = await _context.RefrenceDepositRequest.Where(c => c.RefrenceId == user.Id)
                .Select(c => new
                {
                    c.Id,
                    RefrenceTransationStatus = c.RefrenceTransationStatus,
                    EnumDescription = c.RefrenceTransationStatus.GetEnumDescription(),
                    CreateDate = c.CreateDate.ToPersianDateTextify(true).ToString()
                })
                .ToListAsync();
            return Json(refrenecFromDB);
        }





        ////////[HttpPost("Withdrawl")]
        ////////[AllowAnonymous]
        ////////public async Task<IActionResult> Withdrawl(WithdrawlDTO model)
        ////////{
        ////////    var error = new List<string>();
        ////////    var refrenceFromDB = await _context.Users.FirstOrDefaultAsync(c => c.Id == model.RefrenceId);
        ////////    if (refrenceFromDB == null)
        ////////    {
        ////////        error.Add("نماینده یافت نشد");
        ////////        return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));
        ////////    }
        ////////    if (refrenceFromDB.RefrenceTotalPrice < model.Amount)
        ////////    {
        ////////        error.Add("مبلغ بیش تر از موجودی است");
        ////////        return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));
        ////////    }


        ////////    refrenceFromDB.RefrenceTotalPrice = (decimal)(refrenceFromDB.RefrenceTotalPrice - model.Amount);
        ////////    await _context.SaveChangesAsync();

        ////////    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, refrenceFromDB.RefrenceTotalPrice));
        ////////}




        ///// <summary>
        ///// در خواست  گرفتن پول
        ///// </summary>
        [HttpPost("MakeRequest")]
        //[ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.RefrenceRole)]
        public async Task<IActionResult> MakeRequest(RequestDTO model)
        {

            try {


                var error = new List<string>();
                var userFromDB = await _accountService.GetCurrectUser();

                if (userFromDB == null)
                {
                    error.Add("کاربر نامعتبر");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
                }

                if (userFromDB.Role != PublicHelper.RefrenceRole)
                {
                    error.Add("عدم دسترسی");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
                }

                if (userFromDB.RefrenceTotalPrice < model.Amount)
                {
                    error.Add("مبلغ بیش تر از موجودی است");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));
                }

                var refrenceDepositRequest = new RefrenceDepositRequest()
                {
                    Amount = (decimal)model.Amount,
                    CreateDate = DateTime.Now,
                    RefrenceDescription = model.RefrenceDescription,
                    RefrenceId = userFromDB.Id,
                    RefrenceTransationStatus = RefrenceTransationStatus.Pending,
                    
                    
                };
                await _context.RefrenceDepositRequest.AddAsync(refrenceDepositRequest);
                await _context.SaveChangesAsync();
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, null));


            }
            catch (Exception ex) {
                return Json("");
            }
           
        }
    }





}
