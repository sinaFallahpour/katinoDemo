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
    [Authorize(Roles = PublicHelper.EmployeeRole)]
    [ClaimsAuthorize]
    public class UserJobPreferenceController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IUserJobPreferenceService _UserJobPreferenceService;

        public UserJobPreferenceController(DataContext dataContext,IAccountService accountService,IUserJobPreferenceService UserJobPreferenceService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _UserJobPreferenceService = UserJobPreferenceService;
        }
        [HttpGet("GetUserJobPreferenceForCurrentUser")]
        public async Task<ActionResult> GetUserJobPreferenceForCurrentUser()
        {
            var result =await _UserJobPreferenceService.GetUserJobPreferenceForCurrentUser();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        //[HttpGet("GetUserJobPreferenceDetails")]

        //public async Task<ActionResult> GetUserJobPreferenceDetails(int id)
        //{
        //    var message = new List<string>();
        //    var result =await _UserJobPreferenceService.GetUserJobPreferenceDetailsById(id);
        //    if (result == null)
        //    {
        //        message.Add(" ترجیح شغلی  مورد یافت نشد");
        //        return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
        //    }
        //    message.Add("با موفقیت انجام شد");
        //    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        //}


        //[HttpGet("GetUserJobPreference")]
        //public async Task<ActionResult> GetUserJobPreference(int id)
        //{
        //    var message = new List<string>();
        //    var result =await _UserJobPreferenceService.GetUserJobPreferenceById(id);
        //    if (result == null)
        //    {
        //        message.Add("ترجیح شغلی  یافت نشد");
        //        return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
        //    }
        //    message.Add("با موفقیت انجام شد");
        //    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        //}
        [HttpPost("AddUserJobPreference")]
        public async Task<ActionResult> AddUserJobPreference([FromBody]AddUserJobPreferencesDTO model)
        {
            var message = new List<string>();
            var result =await _UserJobPreferenceService.AddUserJobPreference(model);
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

        [HttpPost("EditUserJobPreference")]
        public async Task<ActionResult> EditUserJobPreference([FromForm] EditUserJobPreferencesDTO model)
        {
            var message = new List<string>();
            var result =await _UserJobPreferenceService.UpdateUserJobPreference(model);
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

        [HttpDelete("DeleteUserJobPreference")]
        public async Task<ActionResult> DeleteUserJobPreference(int id)
        {
            var message = new List<string>();
            var result = await _UserJobPreferenceService.DeleteUserJobPreference(id);
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
