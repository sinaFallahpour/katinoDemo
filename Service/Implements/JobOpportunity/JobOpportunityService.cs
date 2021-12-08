using AutoMapper;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JobOpportunityService : IJobOpportunityService
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IlogService _ilog;
        private readonly IEmailService _emailService;

        public JobOpportunityService(DataContext dataContext, IAccountService accountService
            , IMapper mapper, IlogService ilog, IEmailService emailService)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _mapper = mapper;
            _ilog = ilog;
            _emailService = emailService;
        }
        public async Task<(bool isSuccess, List<string> errors)> CreateJobOpportunity(CreateJobOpportunity model)
        {
            var err = new List<string>();

            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err);
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    err.Add("لطفا ابتدا در قسمت تنظیمات ،ایمیل خود را وارد کنید");
                    return (false, err);
                }
                if (!await _dataContext.Categories.AnyAsync(x => x.Id == model.CategoyId))
                {
                    err.Add("دسته بندی شغلی مورد نظر یافت نشد ");
                    return (false, err);
                }
                var jobAdvertisement = _mapper.Map<CreateJobOpportunity, JobOpportunity>(model);
                jobAdvertisement.EmployeeId = user.Id;
                jobAdvertisement.UpdateDate = DateTime.Now;
                var oldJobOpportunity = await _dataContext.JobOpportunity
                    .Where(x => x.EmployeeId == user.Id).AsNoTracking().FirstOrDefaultAsync();
                if (oldJobOpportunity != null)
                {
                    jobAdvertisement.Id = oldJobOpportunity.Id;
                    _dataContext.JobOpportunity.Update(jobAdvertisement);
                    await _dataContext.SaveChangesAsync();
                }
                else
                {
                    jobAdvertisement.CreatedDate = DateTime.Now;
                    await _dataContext.JobOpportunity.AddAsync(jobAdvertisement);
                    await _dataContext.SaveChangesAsync();
                }

                return (true, null);
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().FullName.ToString(), "CreateJobOpportunity", "JobOpportunity");

                err.Add("مشکلی رخ داده است");
                return (false, err);
            }

        }

        public async Task GetSeggustAdverForUser()
        {

            try
            {
                List<JobAdvertisement> adverList;
                var jobOpportunies = await _dataContext.JobOpportunity.
                    Include(x => x.Employee).AsNoTracking().AsQueryable().ToListAsync();
                //var jobOpportuniesList = await jobOpportunies.ToListAsync();
                var Yesterday = DateTime.Now.AddDays(-1);
                int companyCount = 0;
                foreach (var item in jobOpportunies)
                {
                    adverList = null;
                    var cities = item.City.Split(",");
                    var finalList = new List<SendAdverByMail>();
                    foreach (var city in cities)
                    {
                        var advers = _dataContext.JobAdvertisements
                            .Include(x => x.Category).Include(i => i.Company)
                            //.Where(x => x.CreateAt > Yesterday
                            .Where(x=> x.CreateAt > Yesterday&&
                            /*&&*/ x.CategoryId == item.CategoyId && x.City == city)
                            .OrderByDescending(x => x.IsImmediate)
                            .ThenByDescending(x => x.CreateAt)
                            .AsQueryable().AsNoTracking();



                        var companies =await advers.
                            Select(x => x.CompanyId).Distinct().ToListAsync();
                        companyCount = companies.Count;

                        adverList = await advers.ToListAsync();
                        if (item.WorkExperience != WorkExperience.NotImp)
                        {
                            adverList = adverList.Where(x => x.WorkExperience == item.WorkExperience).ToList();
                        }

                        if (!string.IsNullOrEmpty(item.Title))
                        {
                            var adverWithTitle = adverList.Where(x => x.Title.Contains(item.Title))
                                .ToList();
                            if (adverWithTitle != null) adverList = adverWithTitle;
                        }
                        if (adverList != null)
                        {
                            adverList.AddRange(adverList);
                        }

                        finalList.AddRange( adverList.Select(x => new SendAdverByMail()
                        {
                            CompanyPersianName = x.Company.CompanyPersianName,
                            CompanyEngName = x.Company.CompanyEngName,
                            City = x.City,
                            Id = x.Id,
                            Logo = x.Company.Logo,
                            Title = x.Title,
                            WorkExperience = (x.WorkExperience==WorkExperience.NotImp)?"": x.WorkExperience.GetDisplayAttributeFrom(),
                            IsImmadiate=x.IsImmediate
                        }).ToList());
                       
                        //get first companyName


                    

                    }
                    var user = item.Employee;
                    var userEmail = user.Email;
                    if (companyCount > 0 )
                    {
                        //send Mail
                        string adversHtml = "";
                        var i = finalList.Count;

                        foreach (var adver in finalList)
                        {
                            if (adver.IsImmadiate)
                            {
                                #region ImmediateAdver
                                adversHtml = $@"<tr>
                            <td
                            style='
                            padding: 20px 0;
                            border-right: 2px solid #e24444;
                            border-bottom: 1px solid #cccccc;
                            '
                            >
                            <img
                            style='display: block; width: 50%'
                            src='{PublicHelper.RootUrl}/Img/CompanyLogo/{adver.Logo}'
                            />
                            </td>
                            <td style='padding: 5px; border-bottom: 1px solid #cccccc'>
                            <a
                            style='
                            color: #e24444;
                            display: block;
                            text-align: right;
                            font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            href='/{i}'
                            >
                            <h3>{i}-  {adver.Title}</h3>
                            </a>

                            <div style='margin-top: 20px; margin-bottom: 20px'>
                            <span
                            style='
                            background: #e24444;
                            border-radius: 5px;
                            color: #fff;
                            padding: 5px;
                            '
                            >استخدام فوری</span
                            >
                            </div>

                            <div style='margin-bottom: 10px'>
                            <img
                            style='width: 10px; margin-left: 5px'
                            src='{PublicHelper.RootUrl}/Img/EmailImg/i-location.png'
                            />
                            <span
                            style='
                            font-size: 0.9em;
                            font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            >
                            {adver.CompanyPersianName} | {adver.CompanyEngName}
                            </span>
                            </div>

                            <div style='margin-bottom: 10px'>
                            <img
                            style='width: 10px; margin-left: 5px'
                            src='{PublicHelper.RootUrl}/Img/EmailImg/i-company.png'
                            />
                            <span
                            style='
                            font-size: 0.9em;
                            font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            >
                            {adver.City}
                            </span>
                            </div>

                            <div>
                            <img
                            style='width: 10px; margin-left: 5px'
                            src='{PublicHelper.RootUrl}/Img/EmailImg/i-type.png'
                            />
                            <span
                            style='
                            font-size: 0.9em;
                            font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            >
                            {adver.WorkExperience}
                            </span>
                            </div>
                            </td>
                            <td style='padding: 5px 0; border-bottom: 1px solid #cccccc'>
                            <a
                            style='
                            display: block;
                            width: 100%;
                            margin-bottom: 20px;
                            color: #fff;
                            background-color: #1750ff;
                            border-color: #1750ff;
                            text-align: center;
                            border-radius: 8px;
                            height: 50px;
                            line-height: 50px;
                            text-decoration: none;
                            font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            href='/{i}'
                            >مشاهده</a
                            >
                            </td>
                            </tr>
                            ";
                                 #endregion
                                                        }
                                                        else
                                                        {
                                                            #region Adver
                                                            adversHtml += $@"            <tr>
                            <td
                            style='
                            padding: 20px 0;
                            border-right: 2px solid #cccccc;
                            border-bottom: 1px solid #cccccc;
                            '
                            >
                            <img
                            style='display: block; width: 50%'
                            src='{PublicHelper.RootUrl}/Img/CompanyLogo/{adver.Logo}'
                            />
                            </td>
                            <td style='padding: 20px; border-bottom: 1px solid #cccccc'>
                            <a
                            style='
                            display: block;
                            text-align: right;
                            font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            href='#'
                            >
                            <h3>{i}-  {adver.Title}</h3>
                            </a>

                            <div style='margin-bottom: 10px'>
                            <img
                            style='width: 10px; margin-left: 5px'
                            src='{PublicHelper.RootUrl}/Img/EmailImg/i-location.png'
                            />
                            <span
                            style='
                                font-size: 0.9em;
                                font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            >
                            {adver.CompanyPersianName} | {adver.CompanyEngName}
                            </span>
                            </div>

                            <div style='margin-bottom: 10px'>
                            <img
                            style='width: 10px; margin-left: 5px'
                            src='{PublicHelper.RootUrl}/Img/EmailImg/i-company.png'
                            />
                            <span
                            style='
                                font-size: 0.9em;
                                font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            >
                            {adver.City}
                            </span>
                            </div>

                            <div>
                            <img
                            style='width: 10px; margin-left: 5px'
                            src='{PublicHelper.RootUrl}/Img/EmailImg/i-type.png'
                            />
                            <span
                            style='
                                font-size: 0.9em;
                                font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            >
                            {adver.WorkExperience}
                            </span>
                            </div>
                            </td>
                            <td style='padding: 20px 0; border-bottom: 1px solid #cccccc'>
                            <a
                            style='
                            display: block;
                            width: 100%;
                            margin-bottom: 20px;
                            color: #fff;
                            background-color: #1750ff;
                            border-color: #1750ff;
                            text-align: center;
                            border-radius: 8px;
                            height: 50px;
                            line-height: 50px;
                            text-decoration: none;
                            font-family: Verdana, Geneva, Tahoma, sans-serif;
                            '
                            href='/{i}'
                            >مشاهده</a
                            >
                            </td>
                            </tr>";
                                                            #endregion
                                                        }

                            i--;
                        }

                            #region SendMail
await _emailService.SendEmail(userEmail,"کاتینو: آگهی های مناسب شما", 

@$"
                            <!DOCTYPE html>
                            <html lang='fa'>
                            <head>
                            <meta charset='UTF-8' />
                            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                            <title></title>
                            <link
                            rel='stylesheet'
                            href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css'
                            integrity='sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA=='
                            crossorigin='anonymous'
                            />
                            </head>
                            <body dir='rtl' style='padding: 0; margin: 0; background: #cccccc'>
                            <div style='width: 100% margin: auto; background: #ffffff'>
                            <div style='background: #00bcd4; height: 80px; border: 1px solid #cccccc'>
                            <img
                            src='/logo.png'
                            style='height: 40px; margin-top: 20px; margin-right: 20px'
                            alt='کاتینو'
                            />
                            </div>

                            <div style='padding: 50px 20px; border: 1px solid #cccccc'>
                            <h2
                            style='
                            display: block;
                            text-align: right;
                            margin: 0;
                            padding: 0;
                            font-family: 'Iransans', Tahoma, Geneva, Verdana, sans-serif;
                            font-size: 1em;
                            line-height: 1.6em;
                            '
                            >
                            {user.Fullname} عزیز، شرکت '{finalList.FirstOrDefault().CompanyPersianName}' و  شرکت های دیگر به دنبال استخدام فردی
          مثل شما هستند:
                            </h2>
                            </div>

                            <div style='border: 1px solid #cccccc'>
                            <table style='width: 100%'>
                            <tbody>"
                                                + adversHtml +

                                                @" </tbody>
                            </table>
                            </div>
                            </div>
                            </body>
                            </html>

                            ");
                        #endregion

                    }
                    finalList = null;

                }

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().FullName.ToString(), "GetSeggustAdverForUser", "JobOpportunity");

            }



        }

    }
}
