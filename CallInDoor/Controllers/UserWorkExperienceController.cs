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
using Service.Interfaces.UserWorkExperience;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = PublicHelper.ADMINROLE,Policy =PublicHelper.IsActiveClaim)]
    public class UserWorkExperienceController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserWorkExperienceService _WorkExperienceService;

        public UserWorkExperienceController(DataContext dataContext
            , IUserWorkExperienceService WorkExperienceService)
        {
            _dataContext = dataContext;
            _WorkExperienceService = WorkExperienceService;
        }
      
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        [HttpGet("GetAllWorkExperienceForCuurectUser")]
        public async Task<ActionResult> GetAllWorkExperienceForCuurectUser()
        {
            var result = await _WorkExperienceService.GetAllWorkExperienceForCuurectUser();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetWorkExperience")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> GetWorkExperience(int id)
        {
            var message = new List<string>();
            var result = await _WorkExperienceService.GetWorkExperienceById(id);
            if (result == null)
            {
                message.Add(" سابقه شغلی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("AddWorkExperience")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> AddWorkExperience([FromBody]UserWorkExperienceDTO model)
        {
            var message = new List<string>();
            var result =await _WorkExperienceService.AddWorkExperience(model);
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

        [HttpPost("EditWorkExperience")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> EditWorkExperience([FromBody] UpdateUserWorkExperienceDTO model)
        {
            var message = new List<string>();
            var result =await _WorkExperienceService.UpdateWorkExperience(model);
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

        [HttpDelete("DeleteWorkExperience")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> DeleteWorkExperience(int id)
        {
            var message = new List<string>();
            var result = await _WorkExperienceService.DeleteWorkExperience(id);
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
