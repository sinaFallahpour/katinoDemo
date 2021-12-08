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
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = PublicHelper.ADMINROLE,Policy =PublicHelper.IsActiveClaim)]
    [Authorize(Roles = PublicHelper.EmployeeRole)]
    [ClaimsAuthorize]
    public class EducationalBackgroundController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IEducationalBackgroundService _EducationalBackgroundService;

        public EducationalBackgroundController(DataContext dataContext
            , IEducationalBackgroundService EducationalBackgroundService)
        {
            _dataContext = dataContext;
            _EducationalBackgroundService = EducationalBackgroundService;
        }
      
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        [HttpGet("GetAllEducationalBackgroundForCuurectUser")]
        public async Task<ActionResult> GetAllEducationalBackgroundForCuurectUser()
        {
            var result = await _EducationalBackgroundService.GetAllEducationalBackgroundForCuurectUser();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetEducationalBackground")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> GetEducationalBackground(int id)
        {
            var message = new List<string>();
            var result = await _EducationalBackgroundService.GetEducationalBackgroundById(id);
            if (result == null)
            {
                message.Add(" سابقه تحصیلی  یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("AddEducationalBackground")]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> AddEducationalBackground([FromBody]EducationalBackgroundDTO model)
        {
            var message = new List<string>();
            var result =await _EducationalBackgroundService.AddEducationalBackground(model);
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

        [HttpPost("EditEducationalBackground")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> EditEducationalBackground([FromBody] UpdateEducationalBackgroundDTO model)
        {
            var message = new List<string>();
            var result =await _EducationalBackgroundService.UpdateEducationalBackground(model);
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

        [HttpDelete("DeleteEducationalBackground")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> DeleteEducationalBackground(int id)
        {
            var message = new List<string>();
            var result = await _EducationalBackgroundService.DeleteEducationalBackground(id);
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
