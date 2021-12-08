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

    public class BlogsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IBlogService _blogService;

        public BlogsController(DataContext dataContext,IAccountService accountService,IBlogService blogService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _blogService = blogService;
        }
      
        [HttpGet("GetAllblog")]
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.ADMINROLE)]

        public async Task<ActionResult> GetAllblog()
        {
            var message = new List<string>();
            var result =await _blogService.GetAllBlog();
            if (!result.isSuccess)
            {
                message.Add(" بلاگی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            return new JsonResult(result.result);
        }


        [HttpGet("GetAllBlogForIndex")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllBlogForIndex(BlogType? type)
        {
            var message = new List<string>();
            var result = await _blogService.GetTop10BlogForIndex(type);
            if (!result.isSuccess)
            {
                message.Add(" بلاگی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.result));
        }

      


        [HttpGet("GetblogById")]

        public async Task<ActionResult> GetblogById(int id)
        {
            var message = new List<string>();
            var result = await _blogService.GetBlogById(id);
            if (!result.isSuccess)
            {
                message.Add(" بلاگی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.result));

        }


        [HttpGet("GetContent")]

        public async Task<ActionResult> GetContent(int id)
        {
            var message = new List<string>();
            var result = await _blogService.GetBlogById(id);
            if (!result.isSuccess)
            {
                message.Add(" بلاگی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.result.Content));

        }

        [HttpPost("Createblog")]
        [ClaimsAuthorize]

        [Authorize(Roles = PublicHelper.ADMINROLE)]

        public async Task<ActionResult> Createblog([FromForm] BlogDTO model)
        {
            var message = new List<string>();
            var result = await _blogService.AddBlog(model);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));

        }



        [HttpPost("Editblog")]
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.ADMINROLE)]


        public async Task<ActionResult> Editblog([FromForm] EditBlogDTO model)
        {
            var message = new List<string>();
            var result = await _blogService.UpdateBlog(model);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));

        }




        [HttpPost("Deleteblog")]
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.ADMINROLE)]


        public async Task<ActionResult> Deleteblog(int id)
        {
            var message = new List<string>();
            var result = await _blogService.DeleteBlog(id);
            if (!result.isSuccess)
            {
                message.Add(result.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }





    

    }
}
