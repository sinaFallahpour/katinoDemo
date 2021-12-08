using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO.Plan;
using Domain.DTO.Response;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]

    [Authorize(Roles = PublicHelper.ADMINROLE)]
    public class PlanController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IPlanService _planService;
        private readonly IAccountService _accountService;

        public PlanController(DataContext dataContext, IPlanService planService, IAccountService accountService)
        {
            _dataContext = dataContext;
            _planService = planService;
            _accountService = accountService;
        }

        #region CreatePlan
        [ClaimsAuthorize]
        [HttpPost("CreatePlan")]
        public async Task<ActionResult> CreatePlan([FromBody] CreatePlanDTO model)
        {
            var message = new List<string>();
            var result = await _planService.CreatePlan(model);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                foreach (var err in result.errors)
                {
                    message.Add(err);
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion

        #region EditPlan

        [HttpGet("LoadPlan")]
        [ClaimsAuthorize]
        public async Task<ActionResult> LoadPlan(int planId)
        {
            var message = new List<string>();
            var result = await _planService.LoadPlan(planId);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("خطایی رخ داده است");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
        }
        [HttpPost("EditPlan")]
        [ClaimsAuthorize]
        public async Task<ActionResult> EditPlan([FromBody] EditPlanDTO model)
        {
            var message = new List<string>();
            var result = await _planService.EditPlan(model);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                foreach (var err in result.errors)
                {
                    message.Add(err);
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }



        [HttpPost("EditPlanInAdmin")]
        public async Task<ActionResult> EditPlanInAdmin([FromBody] EditPlanInAdmin model)
        {
            var message = new List<string>();
            var userFromDb = await _dataContext.Users.FindAsync(model.UserId);
            if (userFromDb == null)
            {
                message.Add("کاربر یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }



            if (model.Plans == null)
            {
                userFromDb.PlanId = null;
                await _dataContext.SaveChangesAsync();
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }


            var plan = await _dataContext.Plans.FindAsync(model.Plans);
            if (plan == null)
            {
                message.Add("پنل یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }

            //var result = await _giftCodeService.CreateGiftCode(model);
            //if (!result.isSuccess)
            //{
            //    message.Add(result.message);
            //    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            //}

            userFromDb.PlanId = model.Plans;

            var factor = new Domain.Entities.Factor
            {
                CompanyId = userFromDb.Id,
                IsBackMOney = false,
                IsImmediately = false,
                PaymnetType = PaymnetType.AdminManual,
                PlanId = plan.Id,
                Price = 0,
                TrackingCode = "adminmanual",
                Date = DateTime.Now,
            };
            await _dataContext.AddAsync(factor);

            await _dataContext.SaveChangesAsync();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }

        #endregion

        #region GetAllPlans
        [AllowAnonymous]

        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpGet("GetAllPlansForCompanies")]
        public async Task<ActionResult> GetAllPlansForCompanies()
        {
            var message = new List<string>();
            var result = await _planService.GetAllPlanForCompany();
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("خطایی رخ داده است");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
        }



        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpGet("GetAllPlansForEmployee")]
        public async Task<ActionResult> GetAllPlansForEmployee()
        {
            var message = new List<string>();
            var result = await _planService.GetAllPlanForEmployee();
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("اطلاعاتی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
        }



        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [HttpGet("GetAllPlansForAdmin")]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllPlansForAdmin(int page = 0, int pageSize = PublicHelper.PageSize)
        {
            return Ok(_dataContext.Plans.ToList());
            var message = new List<string>();
            var result = await _planService.GetAllPlanForAdmin(page, pageSize);

            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return new JsonResult(result.ListOfData);
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.ListOfData));
            }
            else
            {
                return new JsonResult(null);

                //message.Add("خطایی رخ داده است");
                //return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
        }

        #endregion

        #region ChangePlanStatus
        [HttpPost("EnanlePlan")]
        [ClaimsAuthorize]

        public async Task<ActionResult> EnanlePla(int planId)
        {
            var message = new List<string>();
            var result = await _planService.EnablePlan(planId);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                foreach (var err in result.errors)
                {
                    message.Add(err);
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        [HttpPost("DisablePlan")]
        public async Task<ActionResult> DisablePlan(int planId)
        {
            var message = new List<string>();
            var result = await _planService.DisablePlan(planId);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                foreach (var err in result.errors)
                {
                    message.Add(err);
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion



        #region GetPlanInfo
        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpGet("GetPlanInfo")]
        [ClaimsAuthorize]

        public async Task<ActionResult> GetPlanInfo()
        {
            var message = new List<string>();
            var result = await _planService.PlanInfoForCompany();
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("خطایی رخ داده است");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
        }


        #endregion
        #region EmployeePlan

        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpGet("GetAllPlanForEmployee")]
        [ClaimsAuthorize]

        public async Task<ActionResult> GetAllPlanForEmployee()
        {
            var message = new List<string>();
            var result = await _planService.GetAllPlanForEmployee();
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("خطایی رخ داده است");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
        }
        #endregion
    }
}
