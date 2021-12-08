using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parbad;
using Parbad.AspNetCore;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{

    [Route("api/[controller]")]

    public class ResomeController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IlogService _ilog;
        private readonly IResomeService _resomeService;
        private readonly IPaymentService _paymentService;

        public ResomeController(DataContext dataContext, IlogService ilog, IResomeService resomeService)
        {
            _dataContext = dataContext;
            _ilog = ilog;
            _resomeService = resomeService;
        }
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]

        #region GetResomePercent
        [HttpGet("GetResomePercent")]

        public async Task<IActionResult> GetResomePercent()
        {
            var error = new List<string>();
            var finalresult = await _resomeService.GetResomePercent();
            if (finalresult != null)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult));

            }
            else
            {
                error.Add("اطلاعاتی یافت نشد ");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }
        #endregion
        #region UploadPDfFile
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpPost("UploadPdfFile")]

        public async Task<IActionResult> UploadPdfFile(UploadResomePDF model)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.SetResomeFile(model);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpGet("LoadResomePDF")]

        public async Task<IActionResult> LoadResomePDF()
        {
            var error = new List<string>();
            var finalresult = await _resomeService.LoadResomePDF();
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.error));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpPost("DeleteResomePDF")]

        public async Task<IActionResult> DeleteResomePDF()
        {
            var error = new List<string>();
            var finalresult = await _resomeService.DeleteResomePDF();
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, false));

            }


        }


        #endregion
        #region addAboutMe
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpPost("AddEmployeeAboutMe")]

        public async Task<IActionResult> AddEmployeeAboutMe([FromBody] AboutMe model)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.AddAboutMe(model.aboutMe);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpGet("LoadEmployeeAboutMe")]

        public async Task<IActionResult> LoadAboutMe()
        {
            var error = new List<string>();
            var finalresult = await _resomeService.LoadAboutMe();
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.error));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }
        #endregion

        #region Asing
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpPost("AsignResomeToAdver")]

        public async Task<IActionResult> AsignResomeToAdver(int adverId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.AsignResomeToAdver(adverId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpPost("AsignResomeToListOfAdvers")]

        public async Task<IActionResult> AsignResomeToListOfAdvers([FromBody]List<int> adversId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.AsignResomeToListOFAdvers(adversId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }
        #endregion


        #region Mark
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("MarkAsignResome")]

        public async Task<IActionResult> MarkAsignResome(int asignId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.MarkedAsignResome(asignId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("UnMarkAsignResome")]

        public async Task<IActionResult> UnMarkAsignResome(int asignId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.UnMarkedAsignResome(asignId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        #endregion

        #region CommentForAsignResome
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("AddCommentForAsignResome")]

        public async Task<IActionResult> AddCommentForAsignResome([FromBody]AddCommentForResome model)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.AddCommentForAsignResome(model);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("DeleteCommentFromAsignResome")]

        public async Task<IActionResult> DeleteCommentFromAsignResome(int asignId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.DeleteCommentForAsignResome(asignId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpGet("LoadCommentForAsignResome")]

        public async Task<IActionResult> LoadCommentForAsignResome(int asignId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.LoadCommentForAsignResome(asignId);
            if (!string.IsNullOrEmpty(finalresult.comment))
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.comment));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }
        #endregion



        #region GetAllResomesInfoForAdver
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("GetAllResomesInfoForAdver")]

        public async Task<IActionResult> GetAllResomesInfoForAdver(int adverId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.GetAllResomesInfoForAdver(adverId);
            if (finalresult!=null)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult));

            }
            else
            {
                error.Add("اطلاعاتی برای این آگهی موجود نیست");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("GetAllResomesInfoWithCount")]

        public async Task<IActionResult> GetAllResomesInfoWithCount(int adverId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.GetAllResomesInfoWitCount(adverId);
            if (finalresult != null)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult));

            }
            else
            {
                error.Add("اطلاعاتی برای این آگهی موجود نیست");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("FilterAllResomesInfoForAdver")]

        public async Task<IActionResult> FilterAllResomesInfoForAdver([FromBody]FilterAsingResome model)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.FilterAllResomesInfoForAdver(model);
            if (finalresult != null)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult));

            }
            else
            {
                error.Add("اطلاعاتی برای این آگهی موجود نیست");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        #endregion



        #region GetUserInfoForResome

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpGet("GetUserShortInfoForResome")]

        public async Task<IActionResult> GetUserShortInfoForResome(int asignId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.GetUserShortInfoForResome(asignId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.result));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }


        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpGet("UserAllInfoForResome")]

        public async Task<IActionResult> UserAllInfoForResome(int resomeId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.UserAllInfoForResome(resomeId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.result));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        #endregion

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpPost("ChangeAsignResomeStatus")]

        public async Task<IActionResult> ChangeAsignResomeStatus(ChangeAsignResomeStatus model)
        {
            var error = new List<string>();
          
            var finalresult = await _resomeService.ChangeAsignResomeStatus(model);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }


        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [HttpGet("GetAsignResomeStatus")]

        public async Task<IActionResult> GetAsignResomeStatus(int asignResomeId)
        {
            var error = new List<string>();

            var finalresult = await _resomeService.GetAsignResomeStatus(asignResomeId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.model));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }



        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpGet("GetAllAsignResomeForEmployee")]

        public async Task<IActionResult> GetAllAsignResomeForEmployee()
        {
            var error = new List<string>();
            var finalresult = await _resomeService.GetAllAsignResomeForEmployee();
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.model));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [HttpGet("GetAsignResomeDetailsForEmployee")]

        public async Task<IActionResult> GetAsignResomeDetailsForEmployee(int asignId)
        {
            var error = new List<string>();
            var finalresult = await _resomeService.GetAsignResomeDetailsForEmployee(asignId);
            if (finalresult.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, finalresult.model));

            }
            else
            {
                error.Add(finalresult.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));

            }


        }


    }


}