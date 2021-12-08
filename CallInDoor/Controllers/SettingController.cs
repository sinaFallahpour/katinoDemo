using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Domain.DTO.Response;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]

    public class SettingController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly ISettingService _settingService;

        public SettingController(DataContext dataContext,IAccountService accountService,ISettingService settingService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _settingService = settingService;
        }

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
            [HttpGet("GetAllSetting")]

        public async Task<ActionResult> GetAllSetting()
        {
            var message = new List<string>();
            var result =await _settingService.GetAllSetting();
            if (!result.isSuccess)
            {
                message.Add(" تنظیماتی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            return new JsonResult(result.model);
        }


        [HttpGet("GetAllInfoForIndex")]

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]

        public async Task<ActionResult> GetAllInfoForIndex()
        {
            var message = new List<string>();
            var result = await _settingService.GetAllInfoForIndex();
            if (!result.isSuccess)
            {
                message.Add(" تنظیماتی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            return new JsonResult(result.model);
        }

        /// <summary>
        /// this action api for front
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("GetAllInfoForIndexForFront")]

        [AllowAnonymous]

        public async Task<ActionResult> GetAllInfoForIndexForFront()
        {
            var message = new List<string>();
            var result = await _settingService.GetAllInfoForIndex();
            if (!result.isSuccess)
            {
                message.Add(" تنظیماتی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            //return new JsonResult(result.model);
        }

        [HttpGet("GetAllInfoForAbouteUs")]
        [AllowAnonymous]

        public async Task<ActionResult> GetAllInfoForAbouteUs()
        {
            var message = new List<string>();
            var result = await _settingService.GetAllInfoForAbouteUs();
            if (!result.isSuccess)
            {
                message.Add(" تنظیماتی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            return new JsonResult(result.model);
        }

        [HttpGet("GetSettingById")]

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetSettingById(int id)
        {
            var message = new List<string>();
            var result = await _settingService.GetSettingById(id);
            if (!result.isSuccess)
            {
                message.Add(" تنظیماتی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));

        }

        [HttpPost("CreateSetting")]

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> CreateSetting([FromBody]CreateSettingDTO model)
        {
            var message = new List<string>();
            var result = await _settingService.CreateSetting(model);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));

        }



        [HttpPost("EditSetting")]

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> EditSetting([FromBody] EditSettingDTO model)
        {
            var message = new List<string>();
            var result = await _settingService.EditSetting(model);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));

        }




        [HttpPost("DeleteSetting")]

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> DeleteSetting(int id)
        {
            var message = new List<string>();
            var result = await _settingService.DeleteSetting(id);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }





        [HttpPost("ChnageIndexSetting")]

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> ChnageIndexSetting([FromForm]ChnageIndexSetting model)
        {
            var message = new List<string>();
            var result = await _settingService.ChnageIndexSetting(model);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }


        [HttpPost("CreateContactUs")]
        [AllowAnonymous]

        public async Task<ActionResult> CreateContactUs([FromBody] ContactUsDTO model)
        {
            var message = new List<string>();
            var result = await _settingService.CreateContactUs(model);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }


        [HttpGet("GetAllContactUs")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllContactUs()
        {
            var message = new List<string>();
            var result = await _settingService.GetAllContactUs();
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            return new JsonResult(result.model);
        }


        [HttpGet("GetContactUsMessage")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetContactUsMessage(int id)
        {
            var message = new List<string>();
            var result = await _settingService.GetContactUsMessage(id);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.content));
        }

        [HttpGet("GetAboutUs")]
        public async Task<ActionResult> GetAboutUs()
        {
            var message = new List<string>();
            var result = await _settingService.GetAboutUs();
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.content));
        }


        [HttpGet("GetKhadamatMa")]
        public async Task<ActionResult> GetKhadamatMa()
        {
            var message = new List<string>();
            var result = await _settingService.GetKhadamatMa();
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.content));
        }

        [HttpGet("GetOnlinePaymentGuid")]
        public async Task<ActionResult> GetOnlinePaymentGuid()
        {
            var message = new List<string>();
            var item = await _dataContext.Settings.FirstOrDefaultAsync(
                    x => x.Key.Equals("onlinepaymentgiude")
                    );
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, item.Value));
        }


        [HttpGet("GetSharayetAkhzNamayande")]
        public async Task<ActionResult> GetSharayetAkhzNamayande()
        {
            var message = new List<string>();
            var result = await _settingService.GetSharayetAkhzNamayande();
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.content));
        }


        [HttpGet("GetEmployeeHelper")]
        public async Task<ActionResult> GetEmployeeHelper()
        {
            var message = new List<string>();
            var result = await _settingService.GetEmployeeHelper();
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.content));
        }

        [HttpGet("GetPolicy")]
        public async Task<ActionResult> GetPolicy()
        {
            var message = new List<string>();
            var result = await _settingService.GetPolicy();
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.content));
        }

    }
}
