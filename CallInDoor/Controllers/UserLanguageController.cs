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
using Microsoft.VisualBasic;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = PublicHelper.EmployerRole)]
    //[ClaimsAuthorize]
    public class UserLanguageController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IUserLanguageService _UserLanguageService;

        public UserLanguageController(DataContext dataContext,IAccountService accountService,IUserLanguageService UserLanguageService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _UserLanguageService = UserLanguageService;
        }
        [HttpGet("GetAllUserLanguagesForCurrentUser")]
        public async Task<ActionResult> GetAllUserLanguagesForCurrentUser()
        {
            var result =await _UserLanguageService.GetAllUserLanguagesForCurrentUser();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetUserLanguageDetails")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]

        public async Task<ActionResult> GetUserLanguageDetails(int id)
        {
            var message = new List<string>();
            var result =await _UserLanguageService.GetUserLanguageDetailsById(id);
            if (result == null)
            {
                message.Add(" زبان مورد یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }


        [HttpGet("GetUserLanguage")]
        public async Task<ActionResult> GetUserLanguage(int id)
        {
            var message = new List<string>();
            var result =await _UserLanguageService.GetUserLanguageById(id);
            if (result == null)
            {
                message.Add("دسته بندی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("AddUserLanguage")]
        public async Task<ActionResult> AddUserLanguage([FromBody]AddUserLanguageDTO model)
        {
            var message = new List<string>();
            var result =await _UserLanguageService.AddUserLanguage(model);
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.An_unhandled_error_occurred, message, false, null));
            }
        }

        [HttpPost("EditUserLanguage")]
        public async Task<ActionResult> EditUserLanguage([FromBody] EditUserLanguageDTO model)
        {
            var message = new List<string>();
            var result =await _UserLanguageService.UpdateUserLanguage(model);
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.An_unhandled_error_occurred, message, false, null));
            }
        }

        [HttpDelete("DeleteUserLanguage")]
        public async Task<ActionResult> DeleteUserLanguage(int id)
        {
            var message = new List<string>();
            var result = await _UserLanguageService.DeleteUserLanguage(id);
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.An_unhandled_error_occurred, message, false, null));
            }
        }

    }
}
