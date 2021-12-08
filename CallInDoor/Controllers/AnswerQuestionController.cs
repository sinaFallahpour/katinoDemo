using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Domain.DTO.AnswerQuestion;
using Domain.DTO.Response;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interfaces.Account;
using Xunit.Sdk;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = PublicHelper.ADMINROLE)]
    public class AnswerQuestionController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IAnswerQuestionService _answerQuestion;

        public AnswerQuestionController(DataContext dataContext
            , IAccountService accountService, IAnswerQuestionService answerQuestion)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _answerQuestion = answerQuestion;
        }
        [AllowAnonymous]
        [HttpGet("GetAllAnswerQuestion")]
        public async Task<ActionResult> GetAllAnswerQuestion()
        {
            var result = await _answerQuestion.GetAllAnswerQuestion();
            var message = new List<string>();

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
                return new JsonResult(result.model);
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));

            }

        }



        [HttpGet("GetAnswerQuestionById")]
        public async Task<ActionResult> GetAnswerQuestionById(int id)
        {
            var result = await _answerQuestion.GetAnswerQuestionById(id);
            var message = new List<string>();

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));

            }

        }


        [HttpPost("CreateAnswerQuestion")]
        public async Task<ActionResult> CreateAnswerQuestion([FromBody]AnswerQuestion model)
        {
            var result = await _answerQuestion.CreateAnswerQuestion(model);
            var message = new List<string>();

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.isSuccess));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, false));

            }

        }


        [HttpPost("EditAnswerQuestion")]
        public async Task<ActionResult> EditAnswerQuestion([FromBody]EditAnswerQuestion model)
        {
            var result = await _answerQuestion.EditAnswerQuestion(model);
            var message = new List<string>();

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.isSuccess));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, false));

            }

        }

        [HttpPost("DeleteAnswerQuestion")]
        public async Task<ActionResult> DeleteAnswerQuestion(int id)
        {
            var result = await _answerQuestion.DeleteAnswerQuestion(id);
            var message = new List<string>();

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.isSuccess));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, false));

            }

        }


    }
}
