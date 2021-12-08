using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service;
using Service.Interfaces.Account;
using Service.Interfaces.GenerateImage;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    public class AdverController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IAdverService _adverService;
        private readonly ISendSmsService _sendSmsService;
        private readonly IGenerateImageService _generateImageService;
        private readonly IWebHostEnvironment _env;


        public AdverController(DataContext dataContext, IAccountService accountService, IAdverService adverService,
            IGenerateImageService generateImageService, IWebHostEnvironment env, ISendSmsService sendSmsService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _adverService = adverService;
            _generateImageService = generateImageService;
            _env = env;
            _sendSmsService = sendSmsService;
        }




        #region 

        [HttpPost("DownlaodAdverImage")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [AllowAnonymous]
        public async Task<ActionResult> DownlaodAdverImage(int id)
        {
            var message = new List<string>();

            var adverFromDB = await _dataContext.JobAdvertisements
                .Where(c => c.Id == id)
                .Include(c => c.Company).Include(x => x.Category)
                .FirstOrDefaultAsync();

            if (adverFromDB == null)
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }

            var fileName = DateTime.Now.Ticks.ToString();
            var uniqueFileName = "/Img/Advers/" + fileName + ".jpg";
            string path = _env.WebRootPath + uniqueFileName;



            var defaultImageAddress = _env.WebRootPath + "/adverTemplate.jpg";

            //var address = _generateImageService.GetDefaultBase64Image(adverFromDB, new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold), Color.Black, Color.LightGray, 840, 700, path);
            var address = _generateImageService.GetDefaultBase64Image(adverFromDB, defaultImageAddress, new Font(FontFamily.GenericSansSerif, 43, FontStyle.Bold), Color.White, Color.LightGray, 650, 650, path);
            adverFromDB.ImageAddress = uniqueFileName;
            await _dataContext.SaveChangesAsync();

            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, uniqueFileName));
        }


        #endregion

        #region CreateAdver
        [HttpPost("CreateAdver")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> CreateAdver([FromBody] AddAdverDTO model)
        {
            var message = new List<string>();
            var result = await _adverService.AddAdver(model);
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion


        #region SaveToDraft

        [HttpPost("SaveAdverToDraft")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> SaveAdverToDraft([FromBody] AddAdverDTO model)
        {

            var message = new List<string>();
            var result = await _adverService.SaveToDraft(model);

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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }

        #endregion


        #region GetAdverById
        [HttpGet("GetAdverById")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.EmployerRole + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAdverById(int id)
        {

            var message = new List<string>();
            var result = await _adverService.GetAdverById(id);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                result.Company = null;
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }


        [HttpPost("StorySazAdver")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> StorySazAdver(int id)
        {
            var message = new List<string>();
            var advert = await _adverService.GetAdverById(id);
            try
            {
                if (advert.IsStorySaz)
                {
                    message.Add("از قبل استوری هست");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
                }
                var user = await _accountService.GetCurrectUser();
                if (user.StorySazCount == 0)
                {
                    message.Add("شما توانایی استوری کردن آگهی را ندارید");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
                }
                advert.IsStorySaz = true;
                user.StorySazCount -= 1;
                await _dataContext.SaveChangesAsync();
                var notif = new StorySizeNotification
                {
                    Date = DateTime.Now,
                    JobAdvertisement = advert,
                    JobAdvertisementId = advert.Id,
                    Company = user,
                    CompanyId = user.Id
                };
                await _dataContext.StorySizeNotifications.AddAsync(notif);
                await _dataContext.SaveChangesAsync();
                message.Add("با موفقیت انجام شد");
                await _sendSmsService.SendMessage("09036904488", JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    ["name"] = user.Fullname,
                    ["companyName"] = user.CompanyPersianName,
                    ["code"] = advert.Id.ToString()
                }), "q1ldszmnrf");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            catch (Exception e)
            {
                message.Add("خطا");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion




        #region AdverDetails

        [HttpGet("AdverDetails")]
        [AllowAnonymous]
        public async Task<ActionResult> AdverDetails(int id)
        {
            var message = new List<string>();
            var result = await _adverService.AdverDetails(id);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }

        #endregion


        #region ChangeAdverStatus
        [HttpPost("ChangeAdverStatus")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.EmployerRole + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> ChangeAdverStatus([FromBody] ChangeAdverStatusDTO model)
        {
            var message = new List<string>();
            (bool isSuccess, List<string> errors) result = (false, null);

            switch (model.AdverStatus)
            {
                case AdverStatus.Active:
                    result = await _adverService.EnableAdver(model.AdverId);
                    break;
                case AdverStatus.Draft:
                    break;
                case AdverStatus.Archive:
                    result = await _adverService.ArchiveAdver(model.AdverId);
                    break;
                case AdverStatus.Finished:
                    result = await _adverService.FinishedAdver(model.AdverId);
                    break;
                case AdverStatus.Disable:
                    result = await _adverService.DisableAdver(model.AdverId);
                    break;
                case AdverStatus.Expired:
                    result = (false, message);
                    break;
                default:
                    break;
            }
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        [HttpPost("ExtendAdver")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> ExtendAdver(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.EnableAgainAdver(adverId);
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }

        [HttpPost("AddAdverFromDraft")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> AddAdverFromDraft(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.AddAdverFromDraft(adverId);


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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        [HttpGet("EmployerDisableAdver")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public IActionResult EmployerDisableAdver(int adverId)
        {
            var message = new List<string>();

            var adver = _dataContext.JobAdvertisements.FirstOrDefault(x => x.Id == adverId);
            adver.IsActive = true;
            _dataContext.SaveChanges();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }
        #endregion


        #region GetAllAdver

        [HttpGet("GetLastAdversForIndex")]
        [AllowAnonymous]
        public async Task<ActionResult> GetLastAdversForIndex(int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var result = await _adverService.GetLastAdversForIndex(page, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                //return new JsonResult(null);

                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        [HttpGet("GetLastImmediatelyAdversForIndex")]
        [AllowAnonymous]
        public async Task<ActionResult> GetLastImmediatelyAdversForIndex(int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var result = await _adverService.GetLastImmediatelyAdversForIndex(page, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }


        [HttpGet("GetAllAdverByFilteringForAdmin")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllAdverByFilteringForAdmin(bool? isImmediatley, AdverStatus? adverStatus, AdverCreatationStatus? adverCreatationStatus,int adverCities, int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var cu = await _accountService.GetCurrectUser();
            var result = cu.Role == PublicHelper.RefrenceRole ?
                await _adverService.GetAllAdverByFilteringForAdmin(isImmediatley, adverStatus, adverCreatationStatus, page, adverCities , pageSize, cu.Id) :
                await _adverService.GetAllAdverByFilteringForAdmin(isImmediatley, adverStatus, adverCreatationStatus, page,adverCities, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return new JsonResult(result.ListOfData);
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return new JsonResult(null);
                //return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }

        [HttpGet("GetAllAdverByFilteringForAdminCount")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllAdverByFilteringForAdminCount(bool? isImmediatley, AdverStatus? adverStatus, AdverCreatationStatus? adverCreatationStatus,int adverCities, int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var cu = await _accountService.GetCurrectUser();
            var result = cu.Role == PublicHelper.RefrenceRole ?
                await _adverService.GetAllAdverByFilteringForAdmin(isImmediatley, adverStatus, adverCreatationStatus, page,adverCities, pageSize, cu.Id) :
                await _adverService.GetAllAdverByFilteringForAdmin(isImmediatley, adverStatus, adverCreatationStatus, page, adverCities, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return new JsonResult(result.PageCount);
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return new JsonResult(null);
                //return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        [HttpGet("GetAllAdverByStatusForCurrectUser")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllAdverByStatusForCurrectUser(AdverStatus adverStatus)
        {
            var message = new List<string>();
            var result = await _adverService.GetAllAdverByStatusForCurrectUser(adverStatus);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }



        [HttpGet("SearchAdverForCurrectUser")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllAdverByStatusForCurrectUser(string key)
        {
            var message = new List<string>();
            var result = await _adverService.SearchAdverForCurrectUser(key);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }



        [HttpGet("GetAllAdverForCurrectUser")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllAdverForCurrectUser()
        {
            var message = new List<string>();
            var result = await _adverService.GetAllAdverForCurrectUser();
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        [HttpGet("GetAllAdverForAdmin")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllAdverForAdmin(int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var cu = await _accountService.GetCurrectUser();
            var result = cu.Role == PublicHelper.RefrenceRole ?
                await _adverService.GetAllAdverForAdmin(page, pageSize, cu.Id) :
                await _adverService.GetAllAdverForAdmin(page, pageSize);

            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return new JsonResult(result.ListOfData);

            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return new JsonResult(null);

            }
        }
        [HttpGet("GetAllMarkedAdverForUser")]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllMarkedAdverForUser(int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var result = await _adverService.GetAllMarkedAdverForUser(page, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return new JsonResult(result.ListOfData);

            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return new JsonResult(null);

            }
        }

        [HttpGet("GetAllAdverForIndex")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllAdverForIndex(int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var result = await _adverService.GetAllAdverForIndex(page, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion


        #region SearchAndFilter
        [HttpGet("SearchAdver")]
        [AllowAnonymous]
        public async Task<ActionResult> SearchAdver(string key, string city, int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            //if (string.IsNullOrEmpty(key))
            //{
            //    message.Add("لطفا متن جستجو را وارد کنید");
            //    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            //}
            var result = await _adverService.SearchAdver(key, city, page, pageSize);

            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }

        [HttpPost("FilterAdver")]
        [AllowAnonymous]
        public async Task<ActionResult> FilterAdver([FromBody] FilterAdverDTO model, int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var result = await _adverService.FilterAdver(model, page, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }


        #endregion

        #region Seggestion
        [HttpPost("SuggestionAdverForUser")]
        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> SuggestionAdverForUser(int page, int pageSize = PublicHelper.PageSize)
        {
            var message = new List<string>();
            var result = await _adverService.SuggestionAdverForUser(page, pageSize);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }


        #endregion
        #region TheSame
        [HttpPost("TheSameAdver")]
        [AllowAnonymous]

        public async Task<ActionResult> TheSameAdver(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.TheSameAdver(adverId);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion

        #region EditAdver
        [HttpGet("LoadAdver")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.EmployerRole + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> LoadAdver(int id)
        {
            var message = new List<string>();
            var result = await _adverService.LoadAdver(id);
            if (result != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }


        [HttpPost("EditAdver")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> EditAdver([FromBody] EditAdverDTO model)
        {
            var message = new List<string>();
            var result = await _adverService.EditAdver(model);
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion


        //extend adver
        #region GetAdverStatus

        [HttpGet("GetAdverStatus")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.EmployerRole + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAdverStatus(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.GetAdverStatus(adverId);

            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));

        }
        [HttpGet("IsActiveAdver")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.EmployerRole + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> IsActiveAdver(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.IsActiveAdver(adverId);

            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));

        }
        #endregion


        #region SetImmediateAdver
        [HttpPost("SetImmediateAdver")]
        //[Authorize(Roles = PublicHelper.EmployerRole)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> SetImmediateAdver(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.SetAddverImmediate(adverId);
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
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion

        #region MarkedAdver
        [HttpPost("MarkAdvder")]
        [Authorize(Roles = PublicHelper.EmployerRole + " , " + PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> MarkAdvder(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.MarkAdver(adverId);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {

                message.Add(result.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }

        [HttpPost("UnMarkAdvder")]
        [Authorize(Roles = PublicHelper.EmployerRole + " , " + PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> UnMarkAdvder(int adverId)
        {
            var message = new List<string>();
            var result = await _adverService.UnMarkAdver(adverId);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {

                message.Add(result.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion
        [HttpPost("GetUserPlanWhenCreateAdver")]
        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.EmployerRole + " , " + PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetUserPlanWhenCreateAdver()
        {
            var message = new List<string>();
            var result = await _adverService.GetUserPlan();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result));
        }
        [HttpPost("ChangeAdverCreateStatus")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        public async Task<ActionResult> ChangeAdverCreateStatus([FromBody] AdminDescriptionForAdver model)
        {
            var message = new List<string>();
            var result = await _adverService.ChangeAdverCreateStatus(model);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                message.Add(result.error);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #region AdverNotif
        [HttpGet("GetAdverNotificationForUser")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAdverNotificationForUser()
        {
            var message = new List<string>();
            var result = await _adverService.GetAdverNotificationForUser();
            if (result.adverNotification != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.adverNotification));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        [HttpGet("GetAllAdverNotificationForUser")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllAdverNotificationForUser()
        {
            var message = new List<string>();
            var result = await _adverService.GetAllAdverNotificationForUser();
            if (result.adverNotification != null)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.adverNotification));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }

        [HttpPost("SeenAdverNotification")]
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> SeenAdverNotification([FromBody] List<int> adverIds)
        {
            var message = new List<string>();
            var result = await _adverService.SeenAdverNotification(adverIds);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, null));
            }
            else
            {
                message.Add("آگهی شغلی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
        }
        #endregion
        #region Template
        [HttpPost("GetTemplate")]
        public IActionResult GetTemplate(int id)
        {
            var adver = _dataContext.JobAdvertisements.FirstOrDefault(x => x.Id == id);
            var templates = _dataContext.ResomeColors.ToList();
            var template = templates.FirstOrDefault(x => x.Id == adver.ResomeColorId);
            return new JsonResult(new { templates, template });
        }
        [HttpPost("ChangeTemplate")]
        public IActionResult ChangeTemplate(int templateId, int id)
        {
            var adver = _dataContext.JobAdvertisements.FirstOrDefault(x => x.Id == id);
            adver.ResomeColorId = templateId;
            _dataContext.SaveChanges();
            return new JsonResult(new { status = '1', message = "با موفقیت انجام شد" });
        }
        #endregion
        [HttpPost("ChangeDetailsByAdmin")]
        public IActionResult ChangeDetailsByAdmin(int adverId,string email,string phoneNumber, [FromForm] string address,string desc,string title)
        {
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, new { adverId , email , phoneNumber , address }));
            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            var adver = _dataContext.JobAdvertisements.FirstOrDefault(x => x.Id == adverId);
            adver.Title = title;
            adver.Email = email;
            adver.PhoneNumber = phoneNumber;
            adver.DescriptionOfJob = desc;
            adver.Address = address;
            _dataContext.SaveChanges();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, null));
        }
    }
}

