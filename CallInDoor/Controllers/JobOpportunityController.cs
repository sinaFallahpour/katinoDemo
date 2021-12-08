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
    public class JobOpportunityController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IJobOpportunityService _jobOpportunityService;

        public JobOpportunityController(DataContext dataContext, IAccountService accountService, IJobOpportunityService jobOpportunityService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _jobOpportunityService = jobOpportunityService;
        }

        [HttpPost("AddUserJobOpportunity")]
        public async Task<ActionResult> AddUserJobOpportunity([FromBody] CreateJobOpportunity model)
        {
            var message = new List<string>();
            var result = await _jobOpportunityService.CreateJobOpportunity(model);
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
