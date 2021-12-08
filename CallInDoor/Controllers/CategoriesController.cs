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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = PublicHelper.ADMINROLE)]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;

        public CategoriesController(DataContext dataContext,IAccountService accountService,ICategoryService categoryService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _categoryService = categoryService;
        }
        [HttpGet("GetAllCategoriesForAdmin")]
        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.EmployerRole)]
        [ClaimsAuthorize]

        public async Task<ActionResult> GetAllCategoriesForAdmin()
        {
            var result = _categoryService.GetAllCategoriesForAdmin();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return new JsonResult(result);
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetAllCategories")]
        [AllowAnonymous]
 

        public async Task<ActionResult> GetAllCategories()
        {
            var result = _categoryService.GetAllCategories();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetAllCategories2")]
        [AllowAnonymous]
 

        public async Task<ActionResult> GetAllCategories2()
        {
            var result = _dataContext.Categories.Include(x => x.Childs).Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                x.CategoryId,
                Children = x.Childs.Select(y => new 
                {
                    y.Id,
                    y.Name
                })
            }).Where(x => x.CategoryId == null).ToList();

            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpGet("GetCategory")]
        [ClaimsAuthorize]

        public async Task<ActionResult> GetCategory(int id)
        {
            var message = new List<string>();
            var result = _categoryService.GetCategoryById(id);
            if (result == null)
            {
                message.Add("دسته بندی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("AddCategory")]
        [ClaimsAuthorize]

        public async Task<ActionResult> AddCategory([FromBody]AddCategoryDTO model)
        {
            var message = new List<string>();
            var result =await _categoryService.AddCategory(model);
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

        [HttpPost("EditCategory")]
        [ClaimsAuthorize]

        public async Task<ActionResult> EditCategory([FromBody] EditCategoryDTO model)
        {
            var message = new List<string>();
            var result =await _categoryService.UpdateCategory(model);
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

        
        [HttpPost("DisableCategory")]
        [ClaimsAuthorize]

        public async Task<ActionResult> DisableCategory(int id)
        {
            var message = new List<string>();
            var result = await _categoryService.DisableCategory(id);
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
        [ClaimsAuthorize]

        [HttpPost("EnableCategory")]
        public async Task<ActionResult> EnableCategory(int id)
        {
            var message = new List<string>();
            var result = await _categoryService.EnableCategory(id);
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
