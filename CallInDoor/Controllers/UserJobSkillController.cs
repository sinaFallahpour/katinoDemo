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
    public class UserJobSkillController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IUserJobSkillService _UserJobSkillService;

        public UserJobSkillController(DataContext dataContext,IAccountService accountService,IUserJobSkillService UserJobSkillService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _UserJobSkillService = UserJobSkillService;
        }
        [HttpGet("GetAllUserJobSkillsForCurrentUser")]
        public async Task<ActionResult> GetAllUserJobSkillsForCurrentUser()
        {
            var result =await _UserJobSkillService.GetAllUserJobSkillForCurrentUser();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetUserJobSkillDetails")]
        //[Authorize(Roles = PublicHelper.ADMINROLE)]

        public async Task<ActionResult> GetUserJobSkillDetails(int id)
        {
            var message = new List<string>();
            var result =await _UserJobSkillService.GetUserJobSkillDetailsById(id);
            if (result == null)
            {
                message.Add(" مهارت مورد یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }


        [HttpGet("GetUserJobSkill")]
        public async Task<ActionResult> GetUserJobSkill(int id)
        {
            var message = new List<string>();
            var result =await _UserJobSkillService.GetUserJobSkillById(id);
            if (result == null)
            {
                message.Add("مهارت یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("AddUserJobSkill")]
        public async Task<ActionResult> AddUserJobSkill([FromBody]AddUserJobSkillDTO model)
        {
            var message = new List<string>();
            var result =await _UserJobSkillService.AddUserJobSkill(model);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.result));
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

        [HttpPost("EditUserJobSkill")]
        public async Task<ActionResult> EditUserJobSkill([FromBody] EditUserJobSkillDTO model)
        {
            var message = new List<string>();
            var result =await _UserJobSkillService.UpdateUserJobSkill(model);
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

        [HttpDelete("DeleteUserJobSkill")]
        public async Task<ActionResult> DeleteUserJobSkill(int id)
        {
            var message = new List<string>();
            var result = await _UserJobSkillService.DeleteUserJobSkill(id);
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
