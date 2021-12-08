using DNTPersianUtils.Core;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ResomeService : IResomeService
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _environment;
        private readonly ISendSmsService _sendSmsService;
        private readonly IlogService _ilog;
        private readonly IMailService _mailService;


        public ResomeService(DataContext dataContext, IAccountService accountService, IWebHostEnvironment environment,IMailService mailService,
            ISendSmsService sendSmsService,
            IlogService ilog)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _environment = environment;
            _sendSmsService = sendSmsService;
            _ilog = ilog;
            _mailService = mailService;
        }

        public async Task<(bool isSuccess, string error)> AddAboutMe(string aboutMe)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                var resome = await _dataContext.Resomes.FindAsync(user.ResomeId);
                resome.AboutMe = aboutMe;
                _dataContext.Resomes.Update(resome);
                await _dataContext.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddAboutMe", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }
        public async Task<(bool isSuccess, string error)> LoadAboutMe()
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                var resome = await _dataContext.Resomes.FindAsync(user.ResomeId);
                if (resome != null) return (true, resome.AboutMe);
                else return (false, "اظلاعاتی یافت نشد");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddAboutMe", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, string error)> AddCommentForAsignResome(AddCommentForResome model)
        {
            try
            {
                if (await _dataContext.CommentAsignResomes.AnyAsync(x => x.AsignResomeId == model.asignId))
                {
                    var comment = await _dataContext.CommentAsignResomes.FirstOrDefaultAsync(x => x.AsignResomeId == model.asignId);

                    comment.Comment = model.comment;
                    _dataContext.CommentAsignResomes.Update(comment);
                    await _dataContext.SaveChangesAsync();

                    return (true, "");

                }
                else
                {
                    var comment = new CommentAsignResome()
                    {
                        AsignResomeId = model.asignId,
                        Date = DateTime.Now,
                        Comment = model.comment
                    };
                    await _dataContext.CommentAsignResomes.AddAsync(comment);
                    await _dataContext.SaveChangesAsync();

                    return (true, "");

                }
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddCommentForAsignResome", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }
        public async Task<(bool isSuccess, string error)> DeleteCommentForAsignResome(int asignId)
        {
            try
            {
                var comment = await _dataContext.CommentAsignResomes.FirstOrDefaultAsync(x => x.AsignResomeId == asignId);
                if (comment == null)
                {
                    var err = "رزومه مورد نظر یافت نشد";
                    return (false, err);

                }

                _dataContext.CommentAsignResomes.Remove(comment);
                await _dataContext.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddCommentForAsignResome", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }
        public async Task<(bool isSuccess, string error)> AsignResomeToAdver(int adverId)
        {
            try
            {
                var adver = await _dataContext.JobAdvertisements.FindAsync(adverId);
                if (adver == null || adver.AdverStatus != AdverStatus.Active || adver.AdverCreatationStatus != AdverCreatationStatus.Accepted)
                {
                    return (false, "آگهی انتخابی شما یافت نشد");
                }
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                var employer = _dataContext.Users.FirstOrDefault(x => x.Id == adver.CompanyId);
                //if (!user.EmployeePlanId.HasValue) return (false, "کاربر گرامی برای ارسال رزومه باید طرح بخرید");

                //var employeeFactor = await _dataContext.EmployeeFactors.OrderByDescending(x => x.Date)
                //                        .FirstOrDefaultAsync(x => x.EmployeeId == user.Id);
                //if ((employeeFactor.Date.AddDays(+1) - DateTime.Today).TotalDays < 0)
                //{
                //    user.EmployeePlanId = null;
                //    _dataContext.Users.Update(user);
                //    await _dataContext.SaveChangesAsync();
                //    return (false, "مدت زمان یکساله استفاده از این طرح به پایان رسیده است");
                //}

                var userResome = await _dataContext.Resomes.FirstOrDefaultAsync(x => x.EmployeeId == user.Id);

                var checkAsignResome = await _dataContext.AsignResomes
                    .AsNoTracking()
                    .Where(w => w.ResomeId == userResome.Id && w.JobAdvertisementId == adverId)
                    .FirstOrDefaultAsync();

                if (checkAsignResome != null )
                {
                    return (false, "شما قبلا رزومه خود را برای  آگهی ارسال کرده اید");
                }

                var userResomePercentCompleted = await GetResomePercent();
                if (string.IsNullOrEmpty(userResome.PDFResome) && userResomePercentCompleted.CompeletePercent <= 20)
                {
                    return (false, "شما قادر به ارسال رزومه نیستید لطفا رزومه خود را تکمیل کنید");
                }

                var asign = new AsignResome()
                {
                    ResomeId = user.ResomeId.Value,
                    UpdateDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    JobAdvertisementId = adverId,
                    AsingResomeStatus = AsingResomeStatus.Pending
                };

                await _dataContext.AsignResomes.AddAsync(asign);
                await _dataContext.SaveChangesAsync();


                var JobAdvertisement = await _dataContext.AsignResomes
                    .AsNoTracking()
                    .Where(x => x.JobAdvertisementId == adverId && x.ResomeId == userResome.Id)
                    .Select(c => new { c.JobAdvertisement.Title, c.JobAdvertisement.Company.UserName, c.JobAdvertisement.Company.Fullname , c.JobAdvertisement.Id , c.ResomeId  })
                    .FirstOrDefaultAsync();

                var advert = _dataContext.JobAdvertisements.FirstOrDefault(x => x.Id == JobAdvertisement.Id);
                var resome = _dataContext.Resomes.Include(x =>x.Employee).FirstOrDefault(x => x.Id == JobAdvertisement.ResomeId);

                var notif = new AdvertismentNotification
                {
                    CreateDate = DateTime.Now,
                    Type = NotificationType.Resome,
                    User = user,
                    UserId = user.Id,
                    JobAdvertisement = adver,
                    JobAdvertisementId = adver.Id,
                    Resome = resome,
                    ResomeId = resome.Id,
                    Employee = resome.Employee,
                    EmployeeId = resome.EmployeeId,
                };
                await _dataContext.AdvertismentNotifications.AddAsync(notif);
                await _dataContext.SaveChangesAsync();

                await _sendSmsService.SendNewResomeComeInToEmployer(JobAdvertisement.Fullname, JobAdvertisement.UserName, JobAdvertisement.Title);

                await _sendSmsService.SendNewResomeComeInToEmployer(JobAdvertisement.Fullname, adver.PhoneNumber, JobAdvertisement.Title);

                _mailService.SimpleSend(employer.Email, JobAdvertisement.Title, JobAdvertisement.Title);

                return (true, "");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SetResomeFile", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, string error)> AsignResomeToListOFAdvers(List<int> adversId)
        {
            try
            {

                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                //if (!user.EmployeePlanId.HasValue) return (false, "کاربر گرامی برای ارسال رزومه باید طرح بخرید");

                //var employeeFactor = await _dataContext.EmployeeFactors.OrderByDescending(x => x.Date)
                //                        .FirstOrDefaultAsync(x => x.EmployeeId == user.Id);
                //if ((employeeFactor.Date.AddDays(+1) - DateTime.Today).TotalDays < 0)
                //{
                //    user.EmployeePlanId = null;
                //    _dataContext.Users.Update(user);
                //    await _dataContext.SaveChangesAsync();
                //    return (false, "مدت زمان یکساله استفاده از این طرح به پایان رسیده است");
                //}

                var userResome = await _dataContext.Resomes.FirstOrDefaultAsync(x => x.EmployeeId == user.Id);
                var userResomePercentCompleted = await GetResomePercent();
                if (string.IsNullOrEmpty(userResome.PDFResome) && userResomePercentCompleted.CompeletePercent <= 20)
                {
                    return (false, "شما قادر به ارسال رزومه نیستید لطفا رزومه خود را تکمیل کنید");
                }

                foreach (var adverId in adversId)
                {
                    var adver = await _dataContext.JobAdvertisements.FindAsync(adverId);
                    if (adver == null || adver.AdverStatus != AdverStatus.Active || adver.AdverCreatationStatus != AdverCreatationStatus.Accepted)
                    {
                        return (false, "آگهی انتخابی شما یافت نشد");
                    }
                    if (await _dataContext.AsignResomes.AnyAsync(x => x.JobAdvertisementId == adverId && x.ResomeId == userResome.Id))
                        return (false, $"شما قبلا رزومه خود را برای  آگهی  {adver.Title} ارسال کرده اید");

                    var asign = new AsignResome()
                    {
                        ResomeId = user.ResomeId.Value,
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        JobAdvertisementId = adverId,
                        AsingResomeStatus = AsingResomeStatus.Pending
                    };

                    var JobAdvertisement = await _dataContext.AsignResomes
                    .AsNoTracking()
                    .Where(x => x.JobAdvertisementId == adverId && x.ResomeId == userResome.Id)
                    .Select(c => new { c.JobAdvertisement.Title, c.JobAdvertisement.Company.UserName, c.JobAdvertisement.Company.Fullname })
                    .FirstOrDefaultAsync();

                    await _sendSmsService.SendNewResomeComeInToEmployer(JobAdvertisement.Fullname, JobAdvertisement.UserName, JobAdvertisement.Title);

                    await _sendSmsService.SendNewResomeComeInToEmployer(JobAdvertisement.Fullname, adver.PhoneNumber, JobAdvertisement.Title);
                    await _dataContext.AsignResomes.AddAsync(asign);
                }
                await _dataContext.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SetResomeFile", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }


        public async Task<ResomeCompeleteLevel> GetResomePercent(string? userId = null)
        {
            var user = new User();
            if (string.IsNullOrEmpty(userId))
            {
                user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {

                    return null;
                }
            }
            else
            {
                user = await _dataContext.Users.FindAsync(userId);
                if (user == null && !user.IsActive)
                {

                    return null;
                }
            }

            var resomeId = user.ResomeId;
            var resome = await _dataContext.Resomes
                .Include(x => x.EducationalBackgrounds)
                .Include(x => x.UserLanguages)
                .Include(x => x.UserJobSkills)
                .Include(x => x.UserWorkExperiences)
                .FirstOrDefaultAsync(x => x.Id == resomeId);
            var finalModel = new ResomeCompeleteLevel()
            {
                aboutMe = string.IsNullOrEmpty(resome.AboutMe) ? 6 : 0,
                EducationalBackgroundId = resome.EducationalBackgrounds.Count == 0 ? 15 : 0,
                UserJobPreferencesId = resome.UserJobPreferencesId == null ? 20 : 0,
                UserLanguageId = resome.UserLanguages.Count == 0 ? 12 : 0,
                UserJobSkillId = resome.UserJobSkills.Count == 0 ? 12 : 0,
                UserWorkExperienceId = resome.UserWorkExperiences.Count == 0 ? 15 : 0,
            };
            if (string.IsNullOrEmpty(user.Military))
            {
                finalModel.checkUserInfo = 0;
            }
            finalModel.CompeletePercent = finalModel.GetCompeletePercent();

            return finalModel;
        }

        public async Task<(bool isSuccess, string error)> MarkedAsignResome(int asignId)
        {
            try
            {
                if (await _dataContext.MarkAsignResomes.AnyAsync(x => x.AsignResomeId == asignId))
                {


                    return (false, "این رزومه قبلا به لیست منتخب شما اضافه شد");

                }
                else
                {
                    var markAsign = new MarkAsignResome()
                    {
                        AsignResomeId = asignId,
                        Date = DateTime.Now,

                    };
                    await _dataContext.MarkAsignResomes.AddAsync(markAsign);
                    await _dataContext.SaveChangesAsync();

                    return (true, "");

                }
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "MarkedAsignResome", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, string error)> SetResomeFile(UploadResomePDF PdfFile)
        {
            string filePdf = "";
            var root = _environment.WebRootPath;
            try
            {
                if (Path.GetExtension(Path.GetFileName(PdfFile.PDF.FileName)) == ".pdf")
                {
                    var uploadResult = FileUploader.UploadPDF(PdfFile.PDF, root + "/PDF/ResomePDF/");

                    if (uploadResult.succsseded) filePdf = uploadResult.result;
                    else return (false, uploadResult.result);
                }
                else if (Path.GetExtension(Path.GetFileName(PdfFile.PDF.FileName)) == ".jpg"
                    ||
                    Path.GetExtension(Path.GetFileName(PdfFile.PDF.FileName)) == ".jpeg"
                    )
                {
                    var uploadResult = FileUploader.UploadImage(PdfFile.PDF, root + "/PDF/ResomePDF/");

                    if (uploadResult.succsseded) filePdf = uploadResult.result;
                    else return (false, uploadResult.result);
                }
                else
                {
                    return (false, "فرمت فایل اشتباه می باشد");
                }


            }
            catch (Exception ex)
            {

                throw;
            }

            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                var resome = await _dataContext.Resomes.FindAsync(user.ResomeId);
                if (!string.IsNullOrEmpty(resome.PDFResome))
                {

                    if (System.IO.File.Exists(root + "/PDF/ResomePDF/" + resome.PDFResome))
                    {
                        System.IO.File.Delete(root + "/PDF/ResomePDF/" + resome.PDFResome);
                    }

                }
                resome.PDFResome = filePdf;


                _dataContext.Resomes.Update(resome);
                await _dataContext.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SetResomeFile", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }

        }

        public async Task<(bool isSuccess, string error)> UnMarkedAsignResome(int asignId)
        {
            try
            {
                if (!await _dataContext.MarkAsignResomes.AnyAsync(x => x.AsignResomeId == asignId))
                {


                    return (false, "شما قادر نیستید این رزومه را از لیست های منتخب خود پاک کنید");

                }
                else
                {
                    var markAsign = await _dataContext.MarkAsignResomes.FirstOrDefaultAsync(x => x.AsignResomeId == asignId);

                    _dataContext.MarkAsignResomes.Remove(markAsign);
                    await _dataContext.SaveChangesAsync();

                    return (true, "");

                }
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UnMarkedAsignResome", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }

        public async Task<(string comment, string error)> LoadCommentForAsignResome(int asignId)
        {
            try
            {
                var comment = await _dataContext.CommentAsignResomes.FirstOrDefaultAsync(x => x.AsignResomeId == asignId);
                if (comment == null)
                {
                    var err = "کامنتی یافت نشد";
                    return (null, err);

                }



                return (comment.Comment, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "LoadCommentForAsignResome", "Resome");

                var err = "مشکلی رخ داده است";
                return (null, err);

            }
        }

        public async Task<List<GetAllResomesInfoForAdver>> GetAllResomesInfoForAdver(int adverId)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                var adver = await _dataContext.JobAdvertisements.FindAsync(adverId);
                if (user.Id != adver.CompanyId)
                {
                    return null;
                }

                if (adver == null)
                {
                    return null;
                }
                var asignResome = await _dataContext.AsignResomes
                     .Where(x => x.JobAdvertisementId == adverId)
                    .Include(x => x.Resome).Include(x => x.CommentAsignResomes).Include(x => x.MarkAsignResomes)
                    .OrderByDescending(x => x.Resome.Employee)
                    .Select(x => new GetAllResomesInfoForAdver()
                    {
                        AsignResomeId = x.Id,
                        Id = adverId,
                        AsingResomeStatus = x.AsingResomeStatus,
                        Date = x.CreateDate.ToShortPersianDateTimeString(true),
                        FullName = x.Resome.Employee.Fullname,
                        HasComment = (x.CommentAsignResomes.Count != 0) ? true : false,
                        IsMark = (x.MarkAsignResomes.Count != 0) ? true : false,
                        JobTitle = (x.Resome.UserJobShortDescriptions.FirstOrDefault() != null) ?
                        x.Resome.UserJobShortDescriptions.FirstOrDefault().JobTitle : "",
                        UserAvatar = x.Resome.Employee.Logo,
                        City = x.Resome.Employee.City,
                        Gender = x.Resome.Employee.Gender,
                        Senioritylevel = x.Resome.UserJobPreferences.Senioritylevel,
                        ResomeId = x.ResomeId,
                        SpecialEmpolyee = x.Resome.Employee.SpecialEmpolyee

                    })
                    .OrderByDescending(x => x.SpecialEmpolyee)
                    .ToListAsync();



                if (asignResome == null)
                {
                    return null;
                }

                return (asignResome);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllResomesInfoForAdver", "Resome");

                return null;

            }
        }

        public async Task<GetAllResomesInfoWitCount> GetAllResomesInfoWitCount(int adverId)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                var adver = await _dataContext.JobAdvertisements.FindAsync(adverId);
                if (user.Id != adver.CompanyId)
                {
                    return null;
                }

                if (adver == null)
                {
                    return null;
                }
                var asignResome = await _dataContext.AsignResomes
                    .Include(x => x.Resome).Include(x => x.CommentAsignResomes).Include(x => x.MarkAsignResomes)
                    .OrderByDescending(x => x.CreateDate)
                    .Where(x => x.JobAdvertisementId == adverId)
                    .Select(x => new GetAllResomesInfoForAdver()
                    {
                        AsignResomeId = x.Id,
                        Id = adverId,
                        AsingResomeStatus = x.AsingResomeStatus,
                        Date = x.CreateDate.ToShortPersianDateTimeString(true),
                        FullName = x.Resome.Employee.Fullname,
                        HasComment = (x.CommentAsignResomes.Count != 0) ? true : false,
                        IsMark = (x.MarkAsignResomes.Count != 0) ? true : false,
                        JobTitle = (x.Resome.UserJobShortDescriptions.FirstOrDefault() != null) ?
                        x.Resome.UserJobShortDescriptions.FirstOrDefault().JobTitle : "",
                        UserAvatar = x.Resome.Employee.Logo,
                        City = x.Resome.Employee.City,
                        Gender = x.Resome.Employee.Gender,
                        Senioritylevel = x.Resome.UserJobPreferences.Senioritylevel,
                        ResomeId = x.ResomeId
                    })
                    .ToListAsync();

                var cities = asignResome.Select(x => x.City).Distinct().ToList();


                if (asignResome == null)
                {
                    return null;
                }
                var finalModel = new Dictionary<string, int>();
                finalModel.Add("All", asignResome.Count);
                finalModel.Add("IsMarked", asignResome.Where(x => x.IsMark).ToList().Count);
                finalModel.Add("HasComment", asignResome.Where(x => x.HasComment).ToList().Count);

                //for Gender
                foreach (Gender gender in Enum.GetValues(typeof(Gender)))
                {
                    finalModel.Add("Gender_" + gender.ToString(), asignResome.Where(x => x.Gender == gender).ToList().Count);
                }
                //for AsingResomeStatus
                foreach (AsingResomeStatus asingResomeStatus in Enum.GetValues(typeof(AsingResomeStatus)))
                {
                    finalModel.Add("AsingResomeStatus_" + asingResomeStatus.ToString(), asignResome.Where(x => x.AsingResomeStatus == asingResomeStatus).ToList().Count);

                }

                //for  Senioritylevel
                foreach (Senioritylevel senioritylevel in Enum.GetValues(typeof(Senioritylevel)))
                {
                    finalModel.Add("Senioritylevel_" + senioritylevel.ToString(), asignResome.Where(x => x.Senioritylevel == senioritylevel).ToList().Count);

                }

                //for  SpecialEmpolyee
                foreach (SpecialEmpolyee specialEmpolyee in Enum.GetValues(typeof(SpecialEmpolyee)))
                {
                    finalModel.Add("SpecialEmpolyee_" + specialEmpolyee.ToString(), asignResome.Where(x => x.SpecialEmpolyee == specialEmpolyee).ToList().Count);

                }

                //for cities
                var cityModel = new List<CityWithCountDTO>();
                foreach (var item in cities)
                {
                    if (!string.IsNullOrEmpty(item))
                        cityModel.Add(new CityWithCountDTO() { CityName = item, Count = asignResome.Where(x => x.City == item).ToList().Count });

                }
                var model = new GetAllResomesInfoWitCount() { City = cityModel, Model = finalModel };
                return model;
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllResomesInfoWitCount", "Resome");

                return null;

            }
        }

        public async Task<List<GetAllResomesInfoForAdver>> FilterAllResomesInfoForAdver(FilterAsingResome model)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                var adver = await _dataContext.JobAdvertisements.FindAsync(model.AdverId);
                if (user.Id != adver.CompanyId)
                {
                    return null;
                }

                if (adver == null)
                {
                    return null;
                }
                var asignResome = await _dataContext.AsignResomes
                    .Include(x => x.Resome).Include(x => x.CommentAsignResomes).Include(x => x.MarkAsignResomes)
                    .OrderByDescending(x => x.CreateDate)
                    .Where(x => x.JobAdvertisementId == model.AdverId)
                    .Select(x => new GetAllResomesInfoForAdver()
                    {
                        AsignResomeId = x.Id,
                        Id = model.AdverId,
                        AsingResomeStatus = x.AsingResomeStatus,
                        Date = x.CreateDate.ToShortPersianDateTimeString(true),
                        FullName = x.Resome.Employee.Fullname,
                        HasComment = (x.CommentAsignResomes.Count != 0) ? true : false,
                        IsMark = (x.MarkAsignResomes.Count != 0) ? true : false,
                        JobTitle = (x.Resome.UserJobShortDescriptions.FirstOrDefault() != null) ?
                        x.Resome.UserJobShortDescriptions.FirstOrDefault().JobTitle : "",
                        UserAvatar = x.Resome.Employee.Logo,
                        City = x.Resome.Employee.City,
                        Gender = x.Resome.Employee.Gender,
                        Senioritylevel = x.Resome.UserJobPreferences.Senioritylevel,
                        ResomeId = x.ResomeId
                    })
                    .ToListAsync();



                if (asignResome == null)
                {
                    return null;
                }
                if (model.HasComment != null) asignResome = asignResome.Where(x => x.HasComment == model.HasComment)
                        .ToList();
                if (model.IsMarked != null) asignResome = asignResome.Where(x => x.IsMark == model.IsMarked).ToList();

                if (model.AsingResomeStatuses != null && model.AsingResomeStatuses.Count != 0)
                    asignResome = asignResome.Where(x => model.AsingResomeStatuses.Any(o => o.Equals(x.AsingResomeStatus))).ToList();

                if (model.Genders != null && model.Genders.Count != 0)
                    asignResome = asignResome.Where(x => model.Genders.Any(o => o.Equals(x.Gender))).ToList();

                if (model.Seniorityleveles != null && model.Seniorityleveles.Count != 0)
                    asignResome = asignResome.Where(x => model.Seniorityleveles.Any(o => o.Equals(x.Senioritylevel)))
                        .ToList();

                if (model.SpecialEmpolyees != null && model.SpecialEmpolyees.Count != 0)
                    asignResome = asignResome.Where(x => model.SpecialEmpolyees.Any(o => o.Equals(x.SpecialEmpolyee)))
                        .ToList();


                if (model.Cities != null && model.Cities.Count != 0) asignResome = asignResome.Where(x => model.Cities
                .Any(o => o.Contains(x.City))).ToList();
                if (!string.IsNullOrEmpty(model.SeacrchKey)) asignResome = asignResome
                        .Where(x => x.City.Contains(model.SeacrchKey) || x.FullName.Contains(model.SeacrchKey)
                        || x.JobTitle.Contains(model.SeacrchKey)).ToList();


                return (asignResome);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "FilterAllResomesInfoForAdver", "Resome");

                return null;

            }
        }

        public async Task<(UserShortInfoForResome result, bool isSuccess, string error)> GetUserShortInfoForResome(int asignId)
        {
            try
            {

                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (null, false, "کابر نامعتبر");
                }

                var finalModel = await _dataContext.AsignResomes.Include(x => x.Resome)
                    .Where(x => x.ResomeId == asignId)
                    .Select(x => new UserShortInfoForResome()
                    {
                        Email = x.Resome.Employee.Email,
                        Avatar = x.Resome.Employee.Logo,
                        PhoneNumber = x.Resome.Employee.PhoneNumber,
                        UserPdf = x.Resome.PDFResome,
                        KatinoProfile = x.ResomeId
                    }).FirstOrDefaultAsync();
                if (finalModel == null) return (null, false, "اطلاعاتی یافت نشد");

                return (finalModel, true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetUserShortInfoForResome", "Resome");

                var err = "مشکلی رخ داده است";
                return (null, false, err);

            }
        }

        public async Task<(UserAllInfoForResome result, bool isSuccess, string error)> UserAllInfoForResome(int resomeId)
        {
            try
            {
                //var resome = await _dataContext.Resomes.FindAsync(resomeId);
                //if (resome == null)
                //{
                //    return (null, false, "رزومه مورد نظر یافت نشد");
                //}
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (null, false, "کابر نامعتبر");
                }
                var finalModel = new UserAllInfoForResome();

                var resome = await _dataContext.Resomes.Include(x => x.Employee)
                    .Where(x => x.Id == resomeId)
                    .Include(x => x.Employee)
                    .Include(x => x.UserLanguages).ThenInclude(o => o.Languag)
                    .Include(x => x.UserJobSkills).ThenInclude(z => z.JobSkill)
                    .Include(x => x.UserJobShortDescriptions)
                    .Include(x => x.UserJobPreferences)
                    .Include(x => x.UserWorkExperiences)
                    .Include(x => x.EducationalBackgrounds)
                    .FirstOrDefaultAsync();

                if (resome == null || resome.Employee == null) return (null, false, "رزومه یافت نشد");



                if (!await _dataContext.AsignResomes
                    .Include(x => x.JobAdvertisement)
                    .AnyAsync(x => x.ResomeId == resome.Id && x.JobAdvertisement.CompanyId == user.Id))
                    return (null, false, "دسترسی برای مشاهده این رزومه ندارید");



                finalModel.ResomeId = resome.Id;
                finalModel.AbouteMe = resome.AboutMe;

                //last companies and eductaionbackground
                var lastCompaniesName = new List<string>();
                var lastEducationBackgroundName = new List<string>();
                foreach (var item in resome.UserWorkExperiences)
                {
                    lastCompaniesName.Add(item.CompanyName);
                }

                foreach (var item in resome.EducationalBackgrounds)
                {
                    lastEducationBackgroundName.Add(item.FieldOfStudy);
                }
                var userPersonalInfoForResome = new UserPersonalInfoForResome()
                {
                    Address = resome.Employee.Address,
                    BirthYear = (resome.Employee.Birthday != null) ? resome.Employee.Birthday.ToShortPersianDateString(true) : "",
                    City = resome.Employee.City,
                    Email = resome.Employee.Email,
                    FullName = resome.Employee.Fullname,
                    Military = resome.Employee.Military,
                    PhoneNumber = resome.Employee.PhoneNumber,
                    Gender = resome.Employee.Gender,
                    IsMarried = (resome.Employee.IsMarried.HasValue) ? resome.Employee.IsMarried.Value : null,
                    ExemptionExpirestionDate = (resome.Employee.ExemptionExpirestionDate != null) ?
                    resome.Employee.ExemptionExpirestionDate.ToShortPersianDateString(true) : "",
                    ExemptionExpirestionRecieveDate = (resome.Employee.ExemptionExpirestionRecieveDate != null) ?
                     resome.Employee.ExemptionExpirestionRecieveDate.ToShortPersianDateString(true) : "",
                    EmploymentStatus = (resome.UserJobShortDescriptions.Count > 0) ? resome.UserJobShortDescriptions.FirstOrDefault().EmploymentStatus : null,
                    JobTitle = (resome.UserJobShortDescriptions.Count > 0) ? resome.UserJobShortDescriptions.FirstOrDefault().JobTitle : "",
                    LastCompanies = lastCompaniesName,
                    LastDegreeOfEducation = lastEducationBackgroundName

                };

                finalModel.UserPersonalInfoForResome = userPersonalInfoForResome;


                finalModel.UserJobSkill = resome.UserJobSkills
                    .Select(x => new ListOfUserJobSkill()
                    {
                        Id = x.Id,
                        JobSkillName = x.JobSkill.Name
                    }).ToList();


                finalModel.UserWorkExperience = resome.UserWorkExperiences.
                    Select(x => new UserWorkExperienceDTO()
                    {
                        CompanyName = x.CompanyName,
                        Description = x.Description,
                        EndDate = x.EndDate.ToShortPersianDateString(),
                        StartDate = x.StartDate.ToShortPersianDateTimeString(),
                        WorkTitle = x.WorkTitle
                    }).ToList();


                finalModel.UserEducationalBackground = resome.EducationalBackgrounds
                    .Select(x => new UpdateEducationalBackgroundDTO()
                    {
                        StartDate = x.StartDate.ToShortPersianDateTimeString(),
                        DegreeOfEducation = x.DegreeOfEducation,
                        Description = x.Description,
                        EndDate = x.EndDate.ToShortPersianDateString(),
                        FieldOfStudy = x.FieldOfStudy,
                        UniversityName = x.UniversityName
                    }).ToList();

                finalModel.ListOfUserLanguage = resome.UserLanguages
                    .Select(x => new ListOfUserLanguage()
                    {
                        LanguageLevel = x.LanguageLevel,
                        LanguageName = x.Languag.Name
                    }).ToList();
                if (resome.UserJobPreferences == null)
                {
                    finalModel.UserJobPreference = null;

                }
                else
                {

                    var userJobPreferences = new UserJobPreferences()
                    {
                        City = resome.UserJobPreferences.City,
                        EducationCourses = resome.UserJobPreferences.EducationCourses,
                        FlexibleWorkingTime = resome.UserJobPreferences.FlexibleWorkingTime,
                        HasMeel = resome.UserJobPreferences.HasMeel,
                        Insurance = resome.UserJobPreferences.Insurance,
                        Promotion = resome.UserJobPreferences.Promotion,
                        Salary = resome.UserJobPreferences.Salary,
                        Senioritylevel = resome.UserJobPreferences.Senioritylevel,
                        TransportationService = resome.UserJobPreferences.TransportationService,
                        TypeOfCooperation = resome.UserJobPreferences.TypeOfCooperation,
                        UserJobPreferenceCategories = resome.UserJobPreferences.UserJobPreferenceCategories
                    };
                    finalModel.UserJobPreference = userJobPreferences;

                }


                if (finalModel == null) return (null, false, "اطلاعاتی یافت نشد");

                return (finalModel, true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UserAllInfoForResome", "Resome");

                var err = "مشکلی رخ داده است";
                return (null, false, err);

            }
        }

        public async Task<(bool isSuccess, string error)> ChangeAsignResomeStatus(ChangeAsignResomeStatus model)
        {
            try
            {

                var text = "";

                if (model.AsingResomeStatus == AsingResomeStatus.Rejected)
                {
                    text = "رد";
                }
                else if (model.AsingResomeStatus == AsingResomeStatus.AcceptedForInterview)
                {
                    text = "تایید برای مصاحبه";
                }
                else if (model.AsingResomeStatus == AsingResomeStatus.Hired)
                {
                    text = "تایید برای استخدام قرارگرفت";
                }


                if (model.AsingResomeStatus == AsingResomeStatus.Rejected)
                {
                    if (model.Description == null)
                    {
                        return (false, "دلیل رد کردن را وارد کنید");

                    }
                }

                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                var asignResome = await _dataContext.AsignResomes.Include(x => x.Resome).ThenInclude(x => x.Employee)
                    .Include(x => x.JobAdvertisement)
                    .FirstOrDefaultAsync(x => x.Id == model.AsignResomeId);
                if (asignResome.JobAdvertisement.CompanyId != user.Id)
                {
                    return (false, "کابر نامعتبر");
                }


                asignResome.AsingResomeStatus = model.AsingResomeStatus;
                asignResome.EmployerDescriptioin = model.Description;
                _dataContext.AsignResomes.Update(asignResome);

                var companyFromDb = await _dataContext.Users.Where(c => c.Id == asignResome.JobAdvertisement.CompanyId)
                            .Select(c => new { c.UserName, c.Fullname }).FirstOrDefaultAsync();


                await _sendSmsService.SendConfirmationStatusToEmployee(text, asignResome.Resome.Employee.UserName , asignResome.JobAdvertisement.Title,asignResome.JobAdvertisementId);

                await _dataContext.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ChangeAsignResomeStatus", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }



        public async Task<(bool isSuccess, string error, List<MyAsignListDTO> model)> GetAllAsignResomeForEmployee()
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر", null);
                }
                var asignResome = _dataContext.AsignResomes
                    .Include(x => x.JobAdvertisement).ThenInclude(x => x.Company)
                    .Where(x => x.ResomeId == user.ResomeId.Value)
                    .OrderByDescending(x => x.UpdateDate)
                    .Select(x => new MyAsignListDTO()
                    {
                        AdverId = x.JobAdvertisementId,
                        AsingResomeStatus = x.AsingResomeStatus,
                        AdverTitle = x.JobAdvertisement.Title,
                        AsignDate = x.CreateDate.ToShortPersianDateTimeString(true),
                        CompanyName = x.JobAdvertisement.Company.CompanyPersianName,
                        AsignId = x.Id
                    }).AsNoTracking().AsQueryable();

                var finalModel = await asignResome.ToListAsync();




                return (true, "", finalModel);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllAsignResomeForEmployee", "Resome");

                //var err = "مشکلی رخ داده است";
                var err = JsonConvert.SerializeObject(ex);
                return (false, err, null);

            }
        }

        public async Task<(bool isSuccess, string error, ResomeAsignDetail model)> GetAsignResomeDetailsForEmployee(int asignId)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر", null);
                }
                var asignResome = await _dataContext.AsignResomes
                    .Include(x => x.JobAdvertisement).ThenInclude(x => x.Company)
                    .Include(c => c.Resome).ThenInclude(x => x.Employee)
                    .Where(x => x.Id == asignId && x.ResomeId == user.ResomeId)
                    .Select(x => new ResomeAsignDetail()
                    {
                        AdverId = x.JobAdvertisementId,
                        AsingResomeStatus = x.AsingResomeStatus,
                        AdverTitle = x.JobAdvertisement.Title,
                        AsignDate = x.CreateDate.ToShortPersianDateTimeString(true),
                        CompanyName = x.JobAdvertisement.Company.CompanyPersianName,
                        KatinoPdfFile = x.Resome.KatinoPDFResome,
                        PdfFile = x.Resome.PDFResome,
                        PhoneNumber = x.Resome.Employee.PhoneNumber
                    }).AsNoTracking().FirstOrDefaultAsync();





                return (true, "", asignResome);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAsignResomeDetailsForEmployee", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err, null);

            }

        }

        public async Task<(bool isSuccess, string error, AsingResomeStatus? model)> GetAsignResomeStatus(int asignResomeId)
        {
            try
            {


                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر", null);
                }
                var asignResome = await _dataContext.AsignResomes
                    .Include(x => x.JobAdvertisement)
                    .FirstOrDefaultAsync(x => x.Id == asignResomeId);
                if (asignResome.JobAdvertisement.CompanyId != user.Id)
                {
                    return (false, "کابر نامعتبر", null);

                }
                if (asignResome == null) return (false, "آیدی نامعتبر", null);

                return (true, "", asignResome.AsingResomeStatus);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ChangeAsignResomeStatus", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err, null);

            }
        }

        public async Task<(bool isSuccess, string error)> LoadResomePDF()
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                var resome = await _dataContext.Resomes.FindAsync(user.ResomeId);
                if (resome == null) return (false, "اطلاعاتی یافت نشد");
                if (string.IsNullOrEmpty(resome.PDFResome)) return (false, "اطلاعاتی یافت نشد");

                return (true, resome.PDFResome);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "LoadResomePDF", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }


        public async Task<(bool isSuccess, string error)> DeleteResomePDF()
        {

            string filePdf = "";
            var root = _environment.WebRootPath;


            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }
                var resome = await _dataContext.Resomes.FindAsync(user.ResomeId);
                if (!string.IsNullOrEmpty(resome.PDFResome))
                {

                    if (System.IO.File.Exists(root + "/PDF/ResomePDF/" + resome.PDFResome))
                    {
                        System.IO.File.Delete(root + "/PDF/ResomePDF/" + resome.PDFResome);
                    }

                }
                resome.PDFResome = filePdf;


                _dataContext.Resomes.Update(resome);
                await _dataContext.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteResomePDF", "Resome");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }
        }
    }
}
