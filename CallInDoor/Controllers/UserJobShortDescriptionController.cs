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
    public class UserJobShortDescriptionController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IUserJobShortDescriptionService _shortDescriptionService;

        public UserJobShortDescriptionController(DataContext dataContext,IAccountService accountService,
            IUserJobShortDescriptionService shortDescriptionService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _shortDescriptionService = shortDescriptionService;
        }
       
        [HttpGet("GetUserShortDescription")]

        public async Task<ActionResult> GetUserShortDescription()
        {
            var message = new List<string>();
            var result =await _shortDescriptionService.LoadUserJobShortDescription();
            if (result.finalModel == null)
            {
                message.Add(result.errors);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.finalModel));
        }


      
        [HttpPost("EditUserShortDescription")]
        public async Task<ActionResult> EditUserShortDescription([FromBody] EditUserJobShortDescriptionDTO model)
        {
            var message = new List<string>();
            var result = await _shortDescriptionService.UpdateUserJobShortDescription(model);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
           
            message.Add(result.errors);
            return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
        }



    }
}
