using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Katino.Models;
using Domain;
using Domain.DTO.Account;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Service.Interfaces.Account;
using Service.Interfaces.JwtManager;
using Microsoft.VisualBasic;
using Service;
using Katino;
using Microsoft.Extensions.DependencyInjection;
using DNTPersianUtils.Core;
using DNTPersianUtils.Core.IranCities;
using Katino.Config.Extentions;
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Net;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using System.Security.Policy;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseControlle
    {



        private readonly DataContext _context;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IAccountService _accountService;
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IJwtManager _jwtGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryService _categoryService;
        private readonly IPaymentService _paymentService;
        private readonly IlogService _ilog;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISendSmsService _sendSmsService;

        public AccountController(UserManager<Domain.Entities.User> userManager,
            SignInManager<Domain.Entities.User> signInManager,
            DataContext context,
               IJwtManager jwtGenerator,
               IHttpContextAccessor httpContextAccessor,
               IStringLocalizer<AccountController> localizer,
                IAccountService accountService,
                ICategoryService categoryService,
                IPaymentService paymentService,
                IlogService ilog,
                IWebHostEnvironment webHostEnvironment,
                ISendSmsService sendSmsService

            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _categoryService = categoryService;
            _paymentService = paymentService;
            _ilog = ilog;
            _webHostEnvironment = webHostEnvironment;
            _sendSmsService = sendSmsService;
        }




        #region uodate Last

        [AllowAnonymous]
        [HttpPost("UpdateLastSeen")]
        public async Task<ActionResult> UpdateLastSeen()
        {
            var curentUserFromDB = await _accountService.GetCurrectUser();
            if (curentUserFromDB == null)
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, new List<string>() { "" }, true, null));


            curentUserFromDB.LastSeen = DateTime.UtcNow.ToLongPersianDateTimeString();
            await _context.SaveChangesAsync();

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, new List<string>() { "" }, true, null));
        }

        #endregion 
        #region register
        [AllowAnonymous]
        [HttpPost("RefrencRegister")]
        public async Task<ActionResult> RefrencRegister([FromBody] RefrenceRegisterDTO model)
        {
            var result = await _accountService.RefrenceRegister(model);
            if (!result.isSuccess)
            {
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, new List<string>() {
                result.message
                }, false, null));
            }
            else
            {
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, new List<string>() {"ثبت نام با موفقیت انجام شد"
                }, true, result.mobile));
            }
        }


        [AllowAnonymous]
        [HttpPost("EmployeeRegister")]
        public async Task<ActionResult> EmployeeRegister([FromBody] RegisterDTO model)
        {



            List<string> message = new List<string>();


            var phonenumber = model.PhoneNumber.Trim();
            var user = await _accountService.FindUserByPhonenumber(phonenumber);
            //await _context.Users.Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefaultAsync();


            var random = new Random();
            var code = random.Next(100000, 999999);
            //var code = 1111;

            if (user != null)
            {
                //کاربری جدید که هم شماره اینم کاربره
                if (user.PhoneNumberConfirmed == true)
                {
                    var errors = new List<string>();
                    errors.Add("شماره موبایل تکراری است");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
                    //return new JsonResult(new { Status = 0, Message = _localizer["PhoneNumber  already  exist."].Value.ToString() });
                }

                if (user.PhoneNumberConfirmed == false)
                {
                    user.verificationCode = code;
                    user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
                    user.Fullname = model.FullName;
                    user.ModifiedDate = DateTime.Now;
                    user.IsActive = true;
                    user.SpecialEmpolyee = model.SpecialEmpolyee;

                    //send code ;
                    //_context.Users.Update(user);
                    //await _context.SaveChangesAsync();

                    //return

                    var newResome = new Resome()
                    {
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        EmployeeId = user.Id
                    };
                    await _context.Resomes.AddAsync(newResome);
                    await _context.SaveChangesAsync();
                    user.ResomeId = newResome.Id;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());

                    message.Add("با موفقیت انجام شد");
                    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, user.UserName));
                }
            }

            var SerialNumber = Guid.NewGuid().ToString().GetHash();

            var newUser = new Domain.Entities.User
            {
                UserName = model.PhoneNumber.Trim(),
                SerialNumber = SerialNumber,
                PhoneNumber = model.PhoneNumber.Trim(),
                Fullname = model.FullName,
                Role = PublicHelper.EmployeeRole,
                verificationCode = code,
                RegisterationDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2),
                IsActive = true,
                SpecialEmpolyee = model.SpecialEmpolyee
            };

            var result = await _userManager.CreateAsync(newUser);
            var resome = new Resome()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                EmployeeId = newUser.Id
            };
            await _context.Resomes.AddAsync(resome);
            await _context.SaveChangesAsync();
            newUser.ResomeId = resome.Id;
            _context.Users.Update(newUser);
            await _context.SaveChangesAsync();

            if (result.Succeeded)
            {

                //send Code

                //return
                await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());

                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, newUser.UserName));

            }
            else
            {
                foreach (var err in result.Errors)
                {
                    message.Add(err.ToString());
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));

            }
        }


        [AllowAnonymous]
        [HttpPost("EmployerRegister")]
        public async Task<ActionResult> EmployerRegister([FromBody] RegisterDTO model)
        {
            List<string> message = new List<string>();


            var phonenumber = model.PhoneNumber.Trim();
            var user = await _accountService.FindUserByPhonenumber(phonenumber);
            //await _context.Users.Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefaultAsync();


            var random = new Random();
            var code = random.Next(100000, 999999);
            //var code = 1111;
            var discount = random.Next(10000, 99999);

            if (user != null)
            {
                //کاربری جدید که هم شماره اینم کاربره
                if (user.PhoneNumberConfirmed == true)
                {
                    var errors = new List<string>();
                    errors.Add("شماره موبایل تکراری است");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
                    //return new JsonResult(new { Status = 0, Message = _localizer["PhoneNumber  already  exist."].Value.ToString() });
                }

                if (user.PhoneNumberConfirmed == false)
                {
                    //var giftCode = new GiftCart()
                    //{
                    //    CreateAt = DateTime.Now,
                    //    Discount = PublicHelper.FirstDiscount,
                    //    EmployerId = user.Id,
                    //    IsUse = false,
                    //    GiftCode = discount.ToString(),
                    //    ExpireTime = DateTime.Now.AddDays(7),
                    //};
                    //await _context.GiftCarts.AddAsync(giftCode);
                    user.verificationCode = code;
                    user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
                    user.Fullname = model.FullName;
                    user.ModifiedDate = DateTime.Now;
                    user.Logo = "defult-employer-logo.jpg";
                    user.IsActive = true;
                    //send code ;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());
                    await _sendSmsService.SendGiftCodeCode(model.FullName, model.PhoneNumber, discount.ToString());


                    message.Add("با موفقیت انجام شد");
                    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, user.UserName));
                }
            }




            var SerialNumber = Guid.NewGuid().ToString().GetHash();

            var newUser = new Domain.Entities.User
            {
                UserName = model.PhoneNumber.Trim(),
                SerialNumber = SerialNumber,
                PhoneNumber = model.PhoneNumber.Trim(),
                Role = PublicHelper.EmployerRole,
                verificationCode = code,
                Fullname = model.FullName,
                verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2),
                ModifiedDate = DateTime.Now,
                RegisterationDate = DateTime.Now,
                IsActive = true

            };

            var result = await _userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                var giftCode = new GiftCart()
                {
                    CreateAt = DateTime.Now,
                    Discount = PublicHelper.FirstDiscount,
                    EmployerId = newUser.Id,
                    //EmployerId = user.Id,
                    IsUse = false,
                    GiftCode = discount.ToString(),
                    ExpireTime = DateTime.Now.AddDays(7),
                };
                await _context.GiftCarts.AddAsync(giftCode);
                await _context.SaveChangesAsync();
                //send Code

                await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());
                await _sendSmsService.SendGiftCodeCode(model.FullName, model.PhoneNumber, discount.ToString());

                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, newUser.UserName));

            }
            else
            {
                foreach (var err in result.Errors)
                {
                    message.Add(err.ToString());
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));

            }
        }

        #endregion







        #region login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var error = new List<string>();
            var random = new Random();
            var code = random.Next(100000, 999999);

            //var code = 1111;

            model.PhoneNumber = model.PhoneNumber.Trim();
            var user = await _accountService.FindUserByPhonenumber(model.PhoneNumber);
            if (user == null)
            {
                error.Add("کاربری یافت نشد");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }

            if (user.Role != model.Role)
            {
                //if (user.UserName == "09116838658")
                //{
                //    await _sendSmsService.SendVerificationCode("", model.PhoneNumber, code.ToString());

                //    var SerialNumber = Guid.NewGuid().ToString().GetHash();

                //    user.SerialNumber = SerialNumber;
                //    user.verificationCode = code;
                //    user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
                //    _context.Users.Update(user);
                //    await _context.SaveChangesAsync();
                //    error.Add("باموفقیت انجام شد");
                //    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, new { PhoneNumber = user.PhoneNumber, Role = user.Role }));
                //}



                if (user.Role == PublicHelper.ADMINROLE)
                {
                    error.Add("کاربر گرامی شما در بخش پنل ادمین وارد شوید");
                    return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
                }

                else if (user.Role == PublicHelper.RefrenceRole)
                {
                    error.Add("کاربر گرامی شما در بخش پنل بازاریاب وارد شوید");
                    return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
                }

                else
                {
                    if (user.Role == PublicHelper.EmployerRole)
                    {
                        error.Add("کاربر گرامی شما در بخش کارفرمایان وارد شوید");
                        return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
                    }
                    else
                    {
                        error.Add("کاربر گرامی شما در بخش کارجویان وارد شوید");
                        return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
                    }
                }

            }
            else
            {
                if (user.Role == PublicHelper.ADMINROLE)
                {
                    user.verificationCodeExpireTime = DateTime.UtcNow.AddYears(1000);
                }
                else
                {
                    await _sendSmsService.SendVerificationCode("", model.PhoneNumber, code.ToString());

                    var SerialNumber = Guid.NewGuid().ToString().GetHash();

                    user.SerialNumber = SerialNumber;
                    user.verificationCode = code;
                    user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                error.Add("باموفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, new
                {
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                }));
            }

        }


        #endregion



        #region  veryfication code

        [AllowAnonymous]
        [HttpPost("VerifyCode")]
        public async Task<IActionResult> Verify([FromBody] VerifyDTO model)
        {
            var error = new List<string>();


            ////
            ///admin force login with my phone number and 111111
            //#region AdminForceLogin
            //if (model.PhoneNumber == "09390867564" && model.VerifyCode == 111111)
            //{
            //    var phone = model.PhoneNumber.Trim();
            //    var userr = await _context.Users.Where(c => c.UserName == phone).FirstOrDefaultAsync();


            //    var userInfos = new Domain.DTO.Account.JwtUser
            //    {
            //        Id = userr.Id,
            //        Token = _jwtGenerator.CreateToken(userr),
            //        UserName = userr.UserName,
            //        Role = userr.Role

            //    };
            //    error.Add("باموفقیت انجام شد");
            //    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, userInfos));
            //}
            #endregion

            var res = await _accountService.CheckVeyficatioCode(model);
            if (res.status == 0)
            {
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, res.erros, false, null));
            }


            var phoneNumber = model.PhoneNumber.Trim();
            var user = await _context.Users.Where(c => c.UserName == phoneNumber).FirstOrDefaultAsync();

            if (!user.PhoneNumberConfirmed)
                user.PhoneNumberConfirmed = true;
            var SerialNumber = Guid.NewGuid().ToString().GetHash();

            user.SerialNumber = SerialNumber;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // TODO: generate token
            var userInfo = new Domain.DTO.Account.JwtUser
            {
                Id = user.Id,
                Token = _jwtGenerator.CreateToken(user),
                UserName = user.UserName,
                Role = user.Role

            };
            /// <summary> 
            ///send GiftCode To USer
            /// </summary> 

            //var code = random.Next(100000, 999999);

            error.Add("باموفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, userInfo));
        }






        #region   RefreshCode  

        [AllowAnonymous]
        [HttpPost("RefreshCode")]
        public async Task<IActionResult> RefreshCode([FromForm] RefreshDTO model)
        {
            var error = new List<string>();

            var phoneNumber = model.PhoneNumber.Trim();
            var user = await _accountService.FindUserByPhonenumber(phoneNumber);

            if (user == null)
            {
                error.Add("کاربری یافت نشد");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }

            var random = new Random();
            var code = random.Next(100000, 999999);

            if (user.Role == PublicHelper.ADMINROLE)
            {
                user.verificationCodeExpireTime = DateTime.UtcNow.AddYears(1000);
            }
            else
            {
                user.verificationCode = code;
                user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();



            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, user.PhoneNumber));
        }

        #endregion


        [HttpPost("sitevisit")]
        public async Task<IActionResult> SiteVisit()
        {
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var browser = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();

            if (ip != "::1")
            {
                var visitor = new Domain.Entities.SiteVisit
                {
                    Browser = browser,
                    IP = ip
                };

                await _context.SiteVisits.AddAsync(visitor);

                _context.SaveChanges();
            }
            return Ok(new ResponseResult((int)Domain.DTO.Response.StatusCode.ok));
        }



        #region ForgetPasswod

        [AllowAnonymous]
        [HttpPost("ForgetPasswod")]
        public async Task<IActionResult> ForgetPasswod([FromBody] ForgetPasswordDTO model)
        {
            var error = new List<string>();
            var phoneNumber = model.PhoneNumber.Trim();
            var user = await _accountService.FindUserByPhonenumber(phoneNumber);
            if (user == null)
            {
                error.Add("کاربری یافت نشد");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }
            /*................................................. question.......................................*/
            if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
            {
                error.Add("شماره تلفن تایید شده نیست");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }


            var random = new Random();
            //var code = random.Next(100000, 999999);
            var code = 1111;
            //send code


            //var token = await _accountService.GeneratePasswordResetToken(user);

            //var passwordResetLink = Url.Action("ResetPassword", "Account",
            //    new { PhoneNumber = model.PhoneNumber, token = token }, Request.Scheme);

            user.verificationCode = code;
            user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();


            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, code));
        }

        [AllowAnonymous]
        [HttpPost("SubmitChangePassword")]
        public async Task<IActionResult> SubmitChangePassword([FromBody] SubmitChangePasswordDTO model)
        {
            var error = new List<string>();
            var verify = new VerifyDTO()
            {
                PhoneNumber = model.PhoneNumber,
                VerifyCode = model.VerifyCode
            };
            var res = await _accountService.CheckVeyficatioCode(verify);
            if (res.status == 0)
            {
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, res.erros, false, null));
            }


            var phoneNumber = model.PhoneNumber.Trim();
            var user = await _context.Users.Where(c => c.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (result.Succeeded)
            {
                var SerialNumber = Guid.NewGuid().ToString().GetHash();
                user.SerialNumber = SerialNumber;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // TODO: generate token
                var userInfo = new Domain.DTO.Account.JwtUser
                {
                    Id = user.Id,
                    Token = _jwtGenerator.CreateToken(user),
                    UserName = user.UserName,
                };

                error.Add("باموفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, userInfo));
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    error.Add(err.ToString());
                }
            }
            return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));


        }



        #endregion



        #region CompanySubmitRegistrstion
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        [HttpPost("CompanySubmitRegistrstion")]
        public async Task<IActionResult> CompanySubmitRegistrstion([FromForm] EmployerProfileDTO model)
        {
            var error = new List<string>();
            var user = await _accountService.GetCurrectUser();
            if (user == null)
            {
                error.Add("کاربر نامعتبر  ");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }
            var companyEngName = model.EngFullName;
            var companyPersianName = model.PersianFullName;
            if (await _context.Users.AnyAsync(x => x.CompanyEngName == companyEngName || x.CompanyPersianName == companyPersianName))
            {
                error.Add("نام شرکت تکراری است");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }


            try
            {
                user.CompanyPersianName = model.PersianFullName;
                user.CompanyEngName = model.EngFullName;
                var root = _webHostEnvironment.WebRootPath;
                user.Logo = (model.Image != null) ? FileUploader.UploadImage(model.Image,
                    root + "/Img/CompanyLogo/").result : "";
                user.EmergencPhone = model.EmergencPhone;
                user.Url = model.url;
                //categorie
                foreach (var item in model.FieldOfActivity)
                {
                    var companycategory = new CompanyCategory()
                    {
                        CategoryId = item,
                        Category = _categoryService.GetCategoryById(item),
                        CompanyId = user.Id,
                        Company = user
                    };
                    if (await _context.Categories.AnyAsync(x => x.Id == item))
                    {
                        if (!_context.CompanyCategories.Any(x => x.CompanyId == user.Id && x.CategoryId == item))
                            await _context.CompanyCategories.AddAsync(companycategory);
                    }

                }
                user.NumberOfStaff = model.NumberOfStaff;
                user.ModifiedDate = DateTime.Now;
                user.IsActive = true;
                //set first Gift Cart
                if (!_context.GiftCarts.Any(x => x.EmployerId == user.Id))
                {
                    var giftCode = "11111";

                    var newGiftCart = new GiftCart()
                    {
                        CreateAt = DateTime.Now,
                        EmployerId = user.Id,
                        Employer = user,
                        IsUse = false,
                        GiftCode = giftCode,
                        Discount = 5,//discount for first register and use discount,
                        ExpireTime = DateTime.Now.AddDays(7)//get from setting,

                    };
                    _context.GiftCarts.Add(newGiftCart);
                    await _context.SaveChangesAsync();
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
            }
            catch (Exception ex)
            {

                error.Add(ex.Message);
                error.Add(ex.StackTrace);
                error.Add(ex.InnerException.ToString());
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }

        }


        #endregion

        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        [HttpPost("EmployeeChangeAvatar")]
        public async Task<IActionResult> EmployeeChangeAvatar([FromForm] EmployeeAvatar model)
        {
            var error = new List<string>();
            var user = await _accountService.GetCurrectUser();
            if (user == null)
            {
                error.Add("کاربر نامعتبر  ");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }

            try
            {
                var root = _webHostEnvironment.WebRootPath;
                var result = FileUploader.UploadImage(model.Image,root + "/Img/CompanyLogo/");
                //if (!string.IsNullOrEmpty(user.Logo) || model.Image != null)
                //{
                //    FileUploader.DeleteFile(root + "/Img/EmployeeAvatar/", user.Logo);
                //}
                if (result.succsseded)
                {
                    user.Logo = result.result;
                }
                else
                {
                    error.Add("خطا");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
                }


                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, user.Logo));
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EmployeeChangeAvatar", "Account");

                error.Add("مشکلی رخ داده است ");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }

        }


        #region EditEmployerProfile
        [Authorize(Roles = PublicHelper.EmployerRole/* Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        [HttpGet("LoadEmployerProfile")]
        public async Task<IActionResult> LoadEmployerProfile()
        {

            var error = new List<string>();
            var user = await _accountService.GetCurrectUser();
            if (user == null)
            {
                error.Add("کاربر نامعتبر  ");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }

            try
            {
                var userprofile = new LoadProfile()
                {
                    Cities = user.City,
                    EmergencPhone = user.EmergencPhone,
                    EngFullName = user.CompanyEngName,
                    Image = user.Logo,
                    NumberOfStaff = user.NumberOfStaff,
                    PersianFullName = user.CompanyPersianName,
                    ShortDescription = user.ShortDescription,
                    url = user.Url,
                    Email = user.Email,
                    ManagmentFullName = user.Fullname,
                    //UserId = user.Id,
                };
                var listOfCategories = new List<ListOfCategoriesForSelect>();
                if (user.CompanyCategorires != null)
                {
                    foreach (var item in user.CompanyCategorires)
                    {
                        var category = await _context.Categories.Where(x => x.Id.Equals(item.CategoryId))
                            .Select(x => new ListOfCategoriesForSelect
                            {
                                Id = x.Id,
                                Name = x.Name
                            }).FirstOrDefaultAsync();
                        listOfCategories.Add(category);
                    }
                }
                userprofile.FieldOfActivity = listOfCategories;
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, userprofile));
            }
            catch (Exception ex)
            {

                error.Add(ex.Message);
                error.Add(ex.StackTrace);
                error.Add(ex.InnerException.ToString());
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }

        }


        [Authorize(Roles = PublicHelper.EmployerRole/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        [HttpPost("EditEmployerProfile")]
        public async Task<IActionResult> EditEmployerProfile([FromForm] EditEmployerProfileDTO model)
        {

            var error = new List<string>();
            var user = await _accountService.GetCurrectUser();//user
            if (user == null)
            {
                error.Add("کاربر نامعتبر  ");
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }
            var root = _webHostEnvironment.WebRootPath;



            try
            {
                user.CompanyPersianName = model.PersianFullName;
                user.CompanyEngName = model.EngFullName;

                if (model.Image != null)
                {
                    if (!string.IsNullOrEmpty(user.Logo)) FileUploader.DeleteFile(root + "/Img/CompanyLogo/", user.Logo);

                    var uploadResult = FileUploader.UploadFile(model.Image, root + "/Img/CompanyLogo/");
                    if (uploadResult.succsseded)
                    {
                        user.Logo = (model.Image != null) ? uploadResult.result : "";

                    }

                    else
                    {
                        error.Add(uploadResult.result);
                        return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
                    }

                }

                var cities = Iran.Cities.ToList();
                var city = cities.FirstOrDefault(x => x.CityDivisionCode.ToString() == model.City);
                var finalCity = city.ProvinceName + "، " + city.CityName;

                user.EmergencPhone = model.EmergencPhone;
                user.Url = model.url;
                user.City = finalCity;
                user.ShortDescription = model.ShortDescription;
                user.Email = model.Email;
                user.Fullname = model.ManagmentFullName;
                //categorie
                //delete old
                var companyCategory = await _context.CompanyCategories.Where(x => x.CompanyId == user.Id).ToListAsync();
                _context.CompanyCategories.RemoveRange(companyCategory);
                await _context.SaveChangesAsync();

                //add new
                if (model.FieldOfActivity.Count > 0)
                {
                    foreach (var item in model.FieldOfActivity)
                    {
                        var companycategory = new CompanyCategory()
                        {
                            CategoryId = item,
                            Category = _categoryService.GetCategoryById(item),
                            CompanyId = user.Id,
                            Company = user
                        };
                        if (!_context.CompanyCategories.Any(x => x.CompanyId == user.Id && x.CategoryId == item))
                            await _context.CompanyCategories.AddAsync(companycategory);
                    }
                }
                user.NumberOfStaff = model.NumberOfStaff;
                user.ModifiedDate = DateTime.Now;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
            }
            catch (Exception ex)
            {

                error.Add(ex.Message);
                error.Add(ex.StackTrace);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }

        }

        [Authorize(Roles = PublicHelper.ADMINROLE/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        [HttpPost("AdminChangeEmployerData")]
        public async Task<IActionResult> AdminChangeEmployerData(string companyPersianName, string companyEngName, string managerFullName, string companyPhone, string managerEmail, string managerPhoneNUmber)
        {
            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            var user = _context.Users.FirstOrDefault(x => x.UserName == managerPhoneNUmber.ToEnglishNumber());
            user.Fullname = managerFullName;
            user.StaticPhoneNumber = companyPhone;
            user.CompanyEngName = companyEngName;
            user.CompanyPersianName = companyPersianName;
            user.Email = managerEmail;
            await _context.SaveChangesAsync();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }

        [HttpGet("GetCities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities()
        {
            var finalModel = Iran.Cities.ToList();
            finalModel = finalModel.OrderBy(x => x.ProvinceName).ThenBy(x => x.CityName).ToList();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, finalModel));

        }
        #endregion









        #region GetAllCompanies
        [HttpGet("GetAllCompanies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _accountService.ListOfCompanies();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, companies));
        }
        [HttpGet("ListOfTop50Companies")]
        [AllowAnonymous]
        public async Task<IActionResult> ListOfTop50Companies()
        {
            var companies = await _accountService.ListOfTop50Companies();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, companies));
        }
        [HttpGet("SearchInCompanies")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchInCompanies(string key)
        {
            var companies = await _accountService.SearchInCompanies(key);
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, companies));
        }
        #endregion



        #region Admins
        [HttpGet("GetAllAdmins")]
        [Authorize(Roles = PublicHelper.ADMINROLE/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _context.Users.Where(x => x.Role == PublicHelper.ADMINROLE)
                .Select(c => new
                {
                    c.UserName,
                    c.Fullname,
                    c.Id,
                    c.IsActive,
                }).ToListAsync();
            return new JsonResult(admins);
        }
        [HttpPost("Delete")]
        [Authorize(Roles = PublicHelper.ADMINROLE/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                await _userManager.DeleteAsync(user);
                return new JsonResult(new { status = "1", message = "با موفقیت انجام شد" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { status = "0", message = "خطا" });
            }
        }




        [HttpPost("CreateAdmin")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<ActionResult> CreateAdmin([FromForm] CreateAdmin model)
        {
            List<string> message = new List<string>();
            var phonenumber = model.UserName.Trim();
            var user = await _accountService.FindUserByPhonenumber(phonenumber);
            //await _context.Users.Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefaultAsync();

            var random = new Random();
            var code = random.Next(100000, 999999);
            //var code = 1111;

            if (user != null)
            {
                //کاربری جدید که هم شماره اینم کاربره
                if (user.PhoneNumberConfirmed == true)
                {
                    var errors = new List<string>();
                    errors.Add("شماره موبایل تکراری است");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
                    //return new JsonResult(new { Status = 0, Message = _localizer["PhoneNumber  already  exist."].Value.ToString() });
                }

            }

            var SerialNumber = Guid.NewGuid().ToString().GetHash();

            var newUser = new Domain.Entities.User
            {
                UserName = model.UserName.Trim(),
                SerialNumber = SerialNumber,
                PhoneNumber = model.UserName.Trim(),
                PhoneNumberConfirmed = true,
                Fullname = model.FullName,
                Role = PublicHelper.ADMINROLE,
                verificationCode = code,
                RegisterationDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2),
                IsActive = model.IsActive,
                AcceptRule = true,


                //StaticPhoneNumber = model.StaticPhoneNumber,
                //ShebaNumber = model.ShebaNumber,
                //Gender = model.Gender,
                //isa = model.IsActive,

                //SpecialEmpolyee = model.SpecialEmpolyee
            };

            var result = await _userManager.CreateAsync(newUser);
            //////var resome = new Resome()
            //////{
            //////    CreateDate = DateTime.Now,
            //////    UpdateDate = DateTime.Now,
            //////    EmployeeId = newUser.Id
            //////};
            //await _context.Resomes.AddAsync(resome);
            await _context.SaveChangesAsync();
            //newUser.ResomeId = resome.Id;
            //_context.Users.Update(newUser);
            //////await _context.SaveChangesAsync();

            if (result.Succeeded)
            {

                //send Code

                //#warnin : sina comment this
                //////////////////await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());

                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, newUser.UserName));

            }
            else
            {
                foreach (var err in result.Errors)
                {
                    message.Add(err.ToString());
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));

            }
        }






        [HttpPost("EditAdmin")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<ActionResult> EditAdmin([FromForm] CreateAdmin model)
        {
            var userFromDB = await _context.Users.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (userFromDB == null)
            {
                var errors = new List<string>();
                errors.Add("کاربر یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
            }

            var userFromDB2 = await _context.Users.FirstOrDefaultAsync(c => c.PhoneNumber == model.UserName && c.Id != userFromDB.Id);
            if (userFromDB2 != null)
            {
                var errors = new List<string>();
                errors.Add("نام کاربری موجود است");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
            }
            userFromDB.Fullname = model.FullName;
            userFromDB.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, null));
        }


        #endregion

        #region GetAllEmployees
        [HttpGet("GetAllEmployees")]
        [Authorize(Roles = PublicHelper.ADMINROLE/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> GetAllEmployees()
        {

            var employees = await _accountService.ListOfEmployees();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, employees));

        }

        [HttpGet("CompanyDetails")]
        public async Task<IActionResult> CompanyDetails(string CompanyEngName)
        {

            var companies = await _accountService.CompanyDetails(CompanyEngName);
            if (companies == null)
            {
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, false, null));

            }
            else
            {
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, companies));

            }

        }
        #endregion



        #region BlocAndUnBlockkUser
        [HttpPost("BlockUser")]
        [Authorize(Roles = PublicHelper.ADMINROLE/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var error = new List<string>();

            var result = await _accountService.BlockUser(userId);
            if (!result.isSuccess)
            {
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, result.message, false, null));

            }
            var user = _accountService.FindUserById(userId);
            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

        }

        [HttpPost("UnBlockUser")]
        [Authorize(Roles = PublicHelper.ADMINROLE/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> UnBlockUser(string userId)
        {
            var error = new List<string>();
            var result = await _accountService.UnblockUser(userId);
            if (!result.isSuccess)
            {
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, result.message, false, null));

            }
            var user = _accountService.FindUserById(userId);
            error.Add("با موفقیت انجام شد");

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

        }
        #endregion


        #region EmployerInfo
        [HttpGet("GetEmployerInfo")]
        [Authorize(Roles = PublicHelper.EmployerRole/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> GetEmployerInfo()
        {
            var error = new List<string>();

            var result = await _accountService.EmployerInfo();
            if (result == null)
            {
                error.Add("خطایی رخ داده است");

                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, error, false, null));

            }
            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

        }
        [HttpPost("EditEmployerInfo")]
        [Authorize(Roles = PublicHelper.EmployerRole/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> EditEmployerInfo([FromForm] EmployerInfo model)
        {
            var error = new List<string>();

            var result = await _accountService.EditEmployerInfo(model);
            if (!result.isSuccess)
            {
                foreach (var er in result.message)
                {
                    error.Add(er);
                }

                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }
            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

        }


        #endregion

        [HttpGet("Test")]
        //[ServiceFilter(typeof(CheckTokenIsValidAttribute))]
        //[Authorize(Roles = PublicHelper.EmployerRole)]
        //[ClaimsAuthorize]
        public async Task<ActionResult> GetCategory(int id)
        {
            return new JsonResult(Iran.Cities.OrderBy(x => x.ProvinceName).ThenBy(x => x.CityName).ToList());
            var message = new List<string>();
            var result = _categoryService.GetCategoryById(id);
            if (result == null)
            {
                message.Add("دسته بندی یافت نشد");
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, message, false, null));
            }

            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }


        [HttpGet("GetRefrenceDashboardInfo")]
        //[ServiceFilter(typeof(CheckTokenIsValidAttribute))]
        [Authorize(Roles = PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetRefrenceDashboardInfo()
        {
            var message = new List<string>();
            var result = await _accountService.GetRefrenceDashboardInfo();
            if (!result.isSucces)
            {
                message.Add(result.message);
                return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }

            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
        }


        #region EmployerInfo
        [HttpGet("GetCompanyLogoInIndex")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyLogoInIndex()
        {
            var error = new List<string>();

            var result = await _accountService.GetCompanyLogoInIndex();
            if (result == null)
            {
                error.Add("موردی یافت نشد");

                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, error, false, null));

            }
            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

        }



        #endregion

        #region GetLastOfOrder
        [HttpGet("GetLastOfOrder")]
        [Authorize(Roles = PublicHelper.EmployerRole/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> GetLastOfOrder()
        {
            var error = new List<string>();

            var result = await _paymentService.GetLastOfOrders();

            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

        }

        [HttpGet("GetOrderDetails")]
        [Authorize(Roles = PublicHelper.EmployerRole/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var error = new List<string>();

            var result = await _paymentService.OrderDetails(orderId);
            if (result == null)
            {
                error.Add("سفارشی یافت نشد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, error, false, null));

            }

            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

        }



        #endregion

        #region PreViewOfFactor
        [HttpGet("GetPreViewOfFactor")]
        [Authorize(Roles = PublicHelper.EmployerRole/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> GetPreViewOfFactor(int orderId)
        {
            var error = new List<string>();

            var result = await _paymentService.PreFactor(orderId);
            if (result == null)
            {
                error.Add("سفارشی یافت نشد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, error, false, null));

            }

            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

        }

        #endregion


        #region EmployeePersonalInformation
        [HttpPost("EditEmployeePersonalInformation")]
        [Authorize(Roles = PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<IActionResult> EditEmployeePersonalInformation([FromBody] EmployeePersonalInformationDTO model)
        {
            var error = new List<string>();

            var result = await _accountService.EditEmployeePersonalInformation(model);
            if (!result.isSuccess)
            {
                foreach (var er in result.message)
                {
                    error.Add(er);
                }
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }

            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

        }

        [HttpGet("LoadEmployeePersonalInformation")]
        [Authorize(Roles = PublicHelper.EmployeeRole/*, Policy = PublicHelper.IsActiveClaim*/)]
        [ClaimsAuthorize]
        public async Task<IActionResult> LoadEmployeePersonalInformation()
        {
            var error = new List<string>();

            var result = await _accountService.GetEmployeePersonalInformation();
            if (result == null)
            {
                error.Add("اطلاعاتی یافت نشد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, error, false, null));

            }

            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

        }



        #endregion

        [ClaimsAuthorize]
        [HttpGet("GetUserRole")]
        public async Task<IActionResult> GetUserRole()
        {
            var error = new List<string>();

            var result = await _accountService.GetUserRole();
            if (!result.isSuccess)
            {
                error.Add(result.message);
                return Unauthorized(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
            }




            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result.message));
        }
        [ClaimsAuthorize]
        [HttpPost("UpdateShebaCode")]
        [Authorize(Roles = PublicHelper.EmployerRole)]

        public async Task<IActionResult> UpdateShebaCode(string shebeCode)
        {

            var error = new List<string>();

            if (!DNTPersianUtils.Core.IranShebaUtils.IsValidIranShebaNumber(shebeCode))
            {
                error.Add("شماره شبا وارد شده صحیح نمیباشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            }
            var result = await _accountService.EditShebaCode(shebeCode);
            if (!result.isSuccess)
            {
                error.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            }


            error.Add("با موفقیت انجام شد");

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result.message));
        }


        [ClaimsAuthorize]
        [HttpGet("LoadShebaCode")]
        [Authorize(Roles = PublicHelper.EmployerRole)]

        public async Task<IActionResult> LoadShebaCode()
        {

            var error = new List<string>();


            var result = await _accountService.LoadShebaCode();
            if (!result.isSuccess)
            {
                error.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            }


            error.Add("با موفقیت انجام شد");

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result.shebaCode));
        }



        [ClaimsAuthorize]
        [HttpPost("RatingCompany")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]

        public async Task<IActionResult> RatingCompany(int rate, string userId)
        {

            var error = new List<string>();


            var result = await _accountService.RatingCompany(rate, userId);
            if (!result.isSuccess)
            {
                error.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            }


            error.Add("با موفقیت انجام شد");

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
        }
        [ClaimsAuthorize]
        [HttpPost()]
        [Authorize(Roles = PublicHelper.EmployerRole + "," + PublicHelper.EmployeeRole)]

        public async Task<IActionResult> GetCityForTicket()
        {
            var error = new List<string>();
            var result = await _accountService.GetCityForTicket();
            if (!result.isSuccess)
            {
                error.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            }
            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result.model));
        }


        [ClaimsAuthorize]
        [HttpGet("IsAdmin")]
        public async Task<IActionResult> IsAdmin()
        {
            var user = await _accountService.GetCurrectUser();
            if (user.Role == PublicHelper.ADMINROLE)
            {
                return Ok();

            }
            else
            {
                return Unauthorized();
            }

        }

        [ClaimsAuthorize]
        [HttpGet("IsRefrence")]
        public async Task<IActionResult> IsRefrence()
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.UserName != "09116838658" && user.Role != PublicHelper.RefrenceRole)
            {
                return Unauthorized();
            }
            return Ok();
        }

        [ClaimsAuthorize]
        [HttpGet("GetAllComapinesForAdmin")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]

        public async Task<IActionResult> GetAllComapinesForAdmin()
        {
            var cu = await _accountService.GetCurrectUser();
            var users = cu.Role == PublicHelper.RefrenceRole ?
                await _accountService.GetAllComapinesForAdmin(cu.Id) :
                await _accountService.GetAllComapinesForAdmin();
            return new JsonResult(users);

        }

        [ClaimsAuthorize]
        [HttpGet("GetAllEmployeeForAdmin")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        public async Task<IActionResult> GetAllEmployeeForAdmin()
        {
            var cu = await _accountService.GetCurrectUser();
            var users = cu.Role == PublicHelper.RefrenceRole ?
                await _accountService.GetAllEmployeeForAdmin(cu.Id) :
                await _accountService.GetAllEmployeeForAdmin();
            return new JsonResult(users);

        }


        /// <summary>
        /// #Notif : sina zad
        /// </summary>
        /// <returns></returns>
        [ClaimsAuthorize]
        [HttpGet("GetAllAgentForAdmin")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        public async Task<IActionResult> GetAllAgentForAdmin()
        {

            var users = await _accountService.GetAllRefrenceForAdmin(true);
            return new JsonResult(users);

            //var users = await _accountService.GetAllRefrenceForAdmin();
            //return new JsonResult(users);

            //////var cu = await _accountService.GetCurrectUser();
            //////var users = cu.Role == PublicHelper.RefrenceRole ?
            //////    await _accountService.GetAllEmployeeForAdmin(cu.Id) :
            //////    await _accountService.GetAllEmployeeForAdmin();
            //////return new JsonResult(users);

        }

        /// <summary>
        /// #Notif : sina zad
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllAgent")]
        public async Task<IActionResult> GetAllAgent()
        {
            var users = await _accountService.GetAllRefrenceForAdmin(false);
            return new JsonResult(users);
            //var users = await _accountService.GetAllRefrenceForAdmin();
            //return new JsonResult(users);

            //////var cu = await _accountService.GetCurrectUser();
            //////var users = cu.Role == PublicHelper.RefrenceRole ?
            //////    await _accountService.GetAllEmployeeForAdmin(cu.Id) :
            //////    await _accountService.GetAllEmployeeForAdmin();
            //////return new JsonResult(users);
        }


        [HttpPost("CreateAgent")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        public async Task<ActionResult> CreateAgent([FromForm] CreateAgentDTO model)
        {
            List<string> message = new List<string>();
            var phonenumber = model.PhoneNumber.Trim();
            var user = await _accountService.FindUserByPhonenumber(phonenumber);
            //await _context.Users.Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefaultAsync();

            var random = new Random();
            var code = random.Next(100000, 999999);
            //var code = 1111;


            if (user != null)
            {
                //کاربری جدید که هم شماره اینم کاربره
                if (user.PhoneNumberConfirmed == true)
                {
                    var errors = new List<string>();
                    errors.Add("شماره موبایل تکراری است");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
                    //return new JsonResult(new { Status = 0, Message = _localizer["PhoneNumber  already  exist."].Value.ToString() });
                }

                //////////if (user.PhoneNumberConfirmed == false)
                //////////{
                //////////    user.verificationCode = code;
                //////////    user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
                //////////    user.Fullname = model.FullName;
                //////////    user.ModifiedDate = DateTime.Now;
                //////////    user.IsActive = true;
                //////////    //user.SpecialEmpolyee = model.SpecialEmpolyee;

                //////////    //send code ;
                //////////    //_context.Users.Update(user);
                //////////    //await _context.SaveChangesAsync();

                //////////    //return

                //////////    var newResome = new Resome()
                //////////    {
                //////////        CreateDate = DateTime.Now,
                //////////        UpdateDate = DateTime.Now,
                //////////        EmployeeId = user.Id
                //////////    };
                //////////    await _context.Resomes.AddAsync(newResome);
                //////////    await _context.SaveChangesAsync();
                //////////    user.ResomeId = newResome.Id;
                //////////    _context.Users.Update(user);
                //////////    await _context.SaveChangesAsync();
                //////////    await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());

                //////////    message.Add("با موفقیت انجام شد");
                //////////    return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, user.UserName));
                //////////}
            }

            var SerialNumber = Guid.NewGuid().ToString().GetHash();

            var newUser = new Domain.Entities.User
            {
                UserName = model.PhoneNumber.Trim(),
                SerialNumber = SerialNumber,
                PhoneNumber = model.PhoneNumber.Trim(),
                PhoneNumberConfirmed = true,
                Fullname = model.FullName,
                Role = PublicHelper.RefrenceRole,
                verificationCode = code,
                RegisterationDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2),
                IsActive = model.IsActive,
                AcceptRule = model.AcceptRule,
                StaticPhoneNumber = model.StaticPhoneNumber,
                ShebaNumber = model.ShebaNumber,
                Address = model.Address,
                City = model.City,
                Province = model.Province,
                Iframe = model.Iframe,
                Description = model.Description
                //Gender = model.Gender,
                //isa = model.IsActive,

                //SpecialEmpolyee = model.SpecialEmpolyee
            };

            var result = await _userManager.CreateAsync(newUser);
            //////var resome = new Resome()
            //////{
            //////    CreateDate = DateTime.Now,
            //////    UpdateDate = DateTime.Now,
            //////    EmployeeId = newUser.Id
            //////};
            //await _context.Resomes.AddAsync(resome);
            await _context.SaveChangesAsync();
            //newUser.ResomeId = resome.Id;
            //_context.Users.Update(newUser);
            //////await _context.SaveChangesAsync();

            if (result.Succeeded)
            {

                //send Code

                //#warnin : sina comment this
                //////////////////await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());

                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, newUser.UserName));

            }
            else
            {
                foreach (var err in result.Errors)
                {
                    message.Add(err.ToString());
                }
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));

            }
        }


        [HttpPost("EditAgent")]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        public async Task<ActionResult> EditAgent([FromForm] CreateAgentDTO model)
        {
            var userFromDB = await _context.Users.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (userFromDB == null)
            {
                var errors = new List<string>();
                errors.Add("کاربر یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
            }
            var userFromDB2 = await _context.Users.FirstOrDefaultAsync(c => c.PhoneNumber == model.PhoneNumber && c.Id != userFromDB.Id);
            if (userFromDB2 != null)
            {
                var errors = new List<string>();
                errors.Add("شماره دیگری موجود است ");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, errors, false, null));
            }

            userFromDB.Fullname = model.FullName;
            //userFromDB.PhoneNumber = model.PhoneNumber;
            userFromDB.StaticPhoneNumber = model.StaticPhoneNumber;
            userFromDB.ShebaNumber = model.ShebaNumber;
            userFromDB.IsActive = model.IsActive;
            userFromDB.AcceptRule = model.AcceptRule;
            userFromDB.Address = model.Address;
            userFromDB.Province = model.Province;
            userFromDB.City = model.City;
            //userFromDB.Description = model.Description;
            userFromDB.Iframe = model.Iframe;
            userFromDB.Description = model.Description;

            await _context.SaveChangesAsync();

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, null));
        }



        [HttpGet("AdminForceLogin")]
        public async Task<IActionResult> AdminForceLogin(string phoneNumber, string verificationCode, string role)
        {
            var intVerificationCode = int.Parse(verificationCode);
            var user = await _context.Users.Where(x => x.PhoneNumber == phoneNumber && x.verificationCode == intVerificationCode
              && x.Role == role).FirstOrDefaultAsync();

            try
            {


                if (user == null)
                {

                    return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, null, false, null));
                }
                //if (user.verificationCodeExpireTime < DateTime.Now)
                //{
                //    return NotFound(new ResponseResult(Domain.DTO.Response.StatusCode.notFound, null, false, null));
                //}

                var userInfo = new
                {
                    FullName = user.Fullname,
                    Email = user.Email,
                    Image = user.Logo,
                    ShortDescription = user.ShortDescription,
                    CompanyPersianName = user.CompanyPersianName,
                    CompanyEngName = user.CompanyEngName,
                    toke = _jwtGenerator.CreateToken(user)
                };


                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, userInfo));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, 1));

            }
        }


        [Route("GetCompanyDesc")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetCompanyDesc(string id)
        {
            var company = _context.Users.SingleOrDefault(x => x.Id == id);
            return new JsonResult(company.Description);
        }
        [Route("EditDesc")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult EditDesc(string id, string desc)
        {
            var company = _context.Users.SingleOrDefault(x => x.Id == id);
            company.Description = desc;
            _context.SaveChanges();
            return new JsonResult(new { status = '1', message = "با موفقیت انجام شد" });
        }
        //test 
        [HttpPost("EditImage")]

        public async Task<IActionResult> EditImage([FromForm] model model)
        {
            var root = _webHostEnvironment.WebRootPath;

            var user = await _context.Users.Where(x => x.PhoneNumber == model.PhoneNumber).FirstOrDefaultAsync();

            user.Logo = (model.Image != null) ? FileUploader.UploadImage(model.Image,
            root + "/Img/CompanyLogo/").result : "";
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, true));
        }


        //[Authorize(Roles = PublicHelper.EmployerRole)]
        //[ClaimsAuthorize]
        [HttpGet("GetStorySazNotif")]
        public IActionResult GetStorySazNotif()
        {
            //var user = _context.Users.FirstOrDefault(x => x.UserName == "09020946867");
            //var advert = _context.JobAdvertisements.FirstOrDefault(x => x.CompanyId == user.Id);
            //var n = new StorySizeNotification
            //{
            //    Company = user,
            //    CompanyId = user.Id,
            //    Date = DateTime.Now,
            //    IsSeen = false,
            //    JobAdvertisement = advert,
            //    JobAdvertisementId = advert.Id
            //};

            //_context.StorySizeNotifications.Add(n);
            //_context.SaveChanges();
            //return Ok();
            var notifs = _context.StorySizeNotifications.Where(x => x.IsSeen == false).Include(x => x.Company).Include(x => x.JobAdvertisement).ToList();
            //foreach (var item in notifs)
            //{
            //    item.IsSeen = true;
            //}
            //_context.SaveChanges();
            return new JsonResult(notifs.Select(x => new
            {
                x.Id,
                x.JobAdvertisement.Title,
                x.JobAdvertisementId,
                x.Company.CompanyPersianName
            }));
        }
        [HttpGet("ToggleStorySazNotifIsSeen")]

        public IActionResult ToggleStorySazNotifIsSeen(int id)
        {
            var notif = _context.StorySizeNotifications.FirstOrDefault(x => x.Id == id);
            notif.IsSeen = true;
            _context.SaveChanges();
            return new JsonResult(new { });
        }
        [HttpPost("GetGet")]

        public async Task<IActionResult> GetGet()
        {

            try
            {
                var dates = new List<DateViewModel>();
                for (int i = 0; i < 7; i++)
                {
                    var date = new DateViewModel
                    {
                        Date = DateTime.Now.AddDays(i).ToPersianDateTextify(),
                        NumbericDate = DateTime.Now.AddDays(i).ToShortPersianDateString(),
                        Time = DateTime.Now.AddDays(i).ToString("HH:mm")
                    };
                    dates.Add(date);
                }
                var startTimeMinute = "47";
                var startTimeMinuteInt = int.Parse(startTimeMinute);

                var startTimeHour = DateTime.Now.ToString("HH");
                var startTimeHourInt = int.Parse(startTimeHour);


                startTimeMinuteInt = startTimeMinuteInt / 15;
                startTimeMinuteInt++;
                if (startTimeMinuteInt > 4)
                {
                    startTimeMinuteInt -= 4;
                    startTimeHourInt++;
                    startTimeMinuteInt = startTimeMinuteInt * 15;
                }
                else
                {
                    startTimeMinuteInt = startTimeMinuteInt * 15;
                }


                var finalModel = new DateTimeViewModel()
                {
                    DateTimes = dates,
                    Times = GetTimes("00:00", "24:00"),
                    TodayTimes = GetTimes(startTimeHourInt.ToString() + ":" + startTimeMinuteInt.ToString(), "24:00"),
                };

                return Ok(finalModel);

            }
            catch (Exception ex)
            {

                return new JsonResult(ex);
            }

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, null, true, true));
        }
        [NonAction]
        public List<string> GetTimes(string startDate, string EndDate)
        {
            var start = startDate.Split(':');
            var end = EndDate.Split(':');
            var startHour = int.Parse(start[0]);
            var startMin = int.Parse(start[1]);

            var endHour = int.Parse(end[0]);
            var endMin = int.Parse(end[1]);

            var finalModel = new List<string>();

            while (endHour > startHour || endHour == startHour && endMin > startMin)
            {
                var startDateHourString = startHour.ToString();
                var startDateMinString = startMin.ToString();
                if (startDateMinString.Length < 2)
                {
                    startDateMinString = "0" + startDateMinString;
                }
                if (startDateHourString.Length < 2)
                {
                    startDateHourString = "0" + startDateHourString;
                }
                finalModel.Add(startDateHourString + ":" + startDateMinString);


                startMin += 15;
                if (startMin == 60)
                {
                    startMin = 0;
                    startHour++;

                }

            }
            return finalModel;

        }

        public class model
        {
            public string PhoneNumber { get; set; }
            public IFormFile Image { get; set; }
        }


    }
}
