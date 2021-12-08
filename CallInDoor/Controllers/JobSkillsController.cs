using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = PublicHelper.ADMINROLE,Policy =PublicHelper.IsActiveClaim)]
    public class JobSkillsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IJobSkillService _jobSkillService;

        public JobSkillsController(DataContext dataContext
            ,IAccountService accountService,IJobSkillService jobSkillService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _jobSkillService = jobSkillService;
        }
        [HttpGet("GetAlljobSkillsForSelect")]
        public async Task<ActionResult> GetAlljobSkillsForSelect()
        {
            var result =await _jobSkillService.GetAllJobSkillsForSelect();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        [HttpGet("GetAlljobSkills")]
        public async Task<ActionResult> GetAlljobSkills()
        {
            var result = await _jobSkillService.GetAllJobSkills();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            return new JsonResult(result);
        }
        [HttpGet("GetJobSkill")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetJobSkill(int id)
        {
            var message = new List<string>();
            var result = await _jobSkillService.GetJobSkillById(id);
            if (result == null)
            {
                message.Add("دسته بندی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("AddJobSkill")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> AddJobSkill([FromBody]AddSkillDTO model)
        {
            var message = new List<string>();
            var result =await _jobSkillService.AddJobSkill(model);
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

        [HttpPost("EditJobSkill")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> EditJobSkill([FromBody] EditSkillDTO model)
        {
            var message = new List<string>();
            var result =await _jobSkillService.UpdateJobSkill(model);
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

        [HttpPost("DisableJobSkill")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> DisableJobSkill(int id)
        {
            var message = new List<string>();
            var result = await _jobSkillService.DisableJobSkill(id);
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
        [HttpPost("EnableJobSkill")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> EnableJobSkill(int id)
        {
            var message = new List<string>();
            var result = await _jobSkillService.EnableJobSkill(id);
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
