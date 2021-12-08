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

    [Authorize(Roles = PublicHelper.ADMINROLE)]
    [AllowAnonymous]

    public class GiftCodeController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IGiftCodeService _giftCodeService;

        public GiftCodeController(DataContext dataContext,IAccountService accountService,IGiftCodeService giftCodeService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _giftCodeService = giftCodeService;
        }
      
    

        [HttpGet("GetAllGiftCodeForAdmin")]
        public async Task<ActionResult> GetAllGiftCodeForAdmin()
        {
            var message = new List<string>();
            var result = await _giftCodeService.GetAllGiftCodeForAdmin();
            if (!result.isSuccess)
            {
                message.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.result));
            return new JsonResult(result.result);
        }

      


        [HttpPost("CreateGiftCode")]


        public async Task<ActionResult> CreateGiftCode([FromBody] CreateGiftCode model)
        {
            var message = new List<string>();
            var result = await _giftCodeService.CreateGiftCode(model);
            if (!result.isSuccess)
            {
                message.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));

        }



      

        [HttpDelete("DeleteGiftCode")]


        public async Task<ActionResult> DeleteGiftCode(int id)
        {
            var message = new List<string>();
            var result = await _giftCodeService.DeleteGiftCode(id);
            if (!result.isSuccess)
            {
                message.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }

    }
}
