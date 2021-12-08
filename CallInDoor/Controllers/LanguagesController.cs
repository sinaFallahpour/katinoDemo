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
    [Authorize(Roles = PublicHelper.ADMINROLE)]
    [ClaimsAuthorize]
    public class LanguagesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly ILanguageService _languageService;

        public LanguagesController(DataContext dataContext,
            IAccountService accountService,
            ILanguageService languageService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _languageService = languageService;
        }
        [HttpGet("GetAllLanguages")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllLanguages()
        {
            var result =await _languageService.GetAllLanguags();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetAllLanguagesForAdmin")]
        public async Task<ActionResult> GetAllLanguagesForAdmin()
        {
            var result = await _languageService.GetAllLanguagsForAdmin();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            return new JsonResult(result);
        }
        [HttpGet("GetLanguage")]
        public async Task<ActionResult> GetLanguage(int id)
        {
            var message = new List<string>();
            var result =await _languageService.GetLanguagById(id);
            if (result == null)
            {
                message.Add("زبان مورد نظر یافت نشد ");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("AddLanguage")]
        public async Task<ActionResult> AddLanguage([FromBody]AddLanguageDTO model)
        {
            var message = new List<string>();
            var result =await _languageService.AddLanguag(model);
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

        [HttpPost("EditLanguage")]
        public async Task<ActionResult> EditLanguage([FromBody] UpdateLanguageDTO model)
        {
            var message = new List<string>();
            var result =await _languageService.UpdateLanguag(model);
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

        [HttpPost("DisableLanguage")]
        public async Task<ActionResult> DisableLanguage(int id)
        {
            var message = new List<string>();
            var result = await _languageService.DisableLanguag(id);
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

        [HttpPost("EnableLanguage")]
        public async Task<ActionResult> EnableLanguage(int id)
        {
            var message = new List<string>();
            var result = await _languageService.EnableLanguag(id);
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
