using DNTPersianUtils.Core;
using Domain;
using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserJobShortDescriptionService : IUserJobShortDescriptionService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;
        private readonly IAccountService _accountService;
        public UserJobShortDescriptionService(DataContext dataContext, IlogService ilog, IAccountService accountService)
        {
            this._Context = dataContext;
            _ilog = ilog;
            _accountService = accountService;
        }




        public async Task<(LoadUserJobShortDescriptionDTO finalModel, string errors)> LoadUserJobShortDescription()
        {
            var err = "";
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (null, "کابر نامعتبر");
                }

                var resomeId = user.ResomeId;
                var lastCompanies = await _Context.UserWorkExperiences.Where(x => x.ResomeId == resomeId&&x.IsActive).ToListAsync();
                var educationBackground = await _Context.EducationalBackgrounds.Where(x => x.ResomeId == resomeId&&x.IsActive).ToListAsync();
                var lastCompaniesName = new List<string>();
                var lastEducationBackgroundName = new List<string>();
                foreach (var item in lastCompanies)
                {
                    lastCompaniesName.Add(item.CompanyName);
                }

                foreach (var item in educationBackground)
                {
                    lastEducationBackgroundName.Add(item.FieldOfStudy);
                }
                var userJobShortDescription = new LoadUserJobShortDescriptionDTO();
                if (await _Context.UserJobShortDescription.AnyAsync(x => x.ResomeId == resomeId))
                {
                    var userShortDec = await _Context.UserJobShortDescription.FirstOrDefaultAsync(x => x.ResomeId == resomeId);
                    userJobShortDescription = new LoadUserJobShortDescriptionDTO()
                    {
                        Id=userShortDec.Id,
                        EmploymentStatus = userShortDec.EmploymentStatus,
                        JobTitle = userShortDec.JobTitle,
                        LastCompanies = lastCompaniesName,
                        LastEducationBackground = lastEducationBackgroundName,
                        UserFullName = user.Fullname
                    };
                    return (userJobShortDescription, "");
                }
                else
                {
                    var newUserShortDec = new UserJobShortDescription()
                    {
                        CreateDate = DateTime.Now,
                        ResomeId = resomeId.Value,
                        UpdateDate = DateTime.Now,
                    };
                    await _Context.UserJobShortDescription.AddAsync(newUserShortDec);
                    await _Context.SaveChangesAsync();
                    userJobShortDescription = new LoadUserJobShortDescriptionDTO()
                    {
                        Id=newUserShortDec.Id,
                        LastCompanies = lastCompaniesName,
                        LastEducationBackground = lastEducationBackgroundName,
                        UserFullName = user.Fullname
                    };
                    return (userJobShortDescription, "");

                }

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "LoadUserJobShortDescription", "UserJobShortDescription");

                err = "مشکلی رخ داده است";
                return (null, err);

            }
        }

        public async Task<(bool isSuccess, string errors)> UpdateUserJobShortDescription(EditUserJobShortDescriptionDTO model)
        {
            var err = "";
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    return (false, "کابر نامعتبر");
                }

                var resomeId = user.ResomeId;

                var userJobDescription = await _Context.UserJobShortDescription.FindAsync(model.Id);
                if (userJobDescription == null)
                {
                    err = "مشکلی رخ داده است";
                    return (false, err);
                }
                userJobDescription.JobTitle = model.JobTitle;
                userJobDescription.EmploymentStatus = model.EmploymentStatus;
                userJobDescription.UpdateDate = DateTime.Now;


                 _Context.UserJobShortDescription.Update(userJobDescription);
                await _Context.SaveChangesAsync();
               
                return (true, "");


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditUserJobShortDescriptionDTO", "UserJobShortDescription");

                err = "مشکلی رخ داده است";
                return (false, err);

            }
        }
    }
}
