using DNTPersianUtils.Core;
using Domain;
using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserLanguageService : IUserLanguageService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;
        private readonly IAccountService _accountService;

        public UserLanguageService(DataContext dataContext, IlogService ilog,
            IAccountService accountService)
        {
            this._Context = dataContext;
            _accountService = accountService;
        }

        public async Task<List<ListOfUserLanguage>> GetAllUserLanguagesForCurrentUser()
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserLanguagelist = _Context.UserLanguage.Include(x => x.Languag)
                .Where(x => x.ResomeId == resomeId)
                          .Select(x => new ListOfUserLanguage
                          {
                              LanguageName = x.Languag.Name,
                              Id = x.Id,
                              LanguageLevel = x.LanguageLevel
                          })
                          .AsNoTracking().AsQueryable();

            var finalModel = await UserLanguagelist.ToListAsync();
            return finalModel;
        }



        public async Task<ListOfUserLanguage> GetUserLanguageById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserLanguage = await _Context.UserLanguage.Include(x => x.Languag)
                .Where(x => x.Id == id && x.ResomeId == resomeId)
                .Select(x=>new ListOfUserLanguage()
                {
                    Id=x.Id,
                    LanguageLevel=x.LanguageLevel,
                    LanguageName=x.Languag.Name
                })
                .FirstOrDefaultAsync();
            return UserLanguage;
        }

        public async Task<ListOfUserLanguage> GetUserLanguageDetailsById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserLanguage = await _Context.UserLanguage.Include(x => x.Languag)
                .Where(x => x.Id == id && x.ResomeId == resomeId)
                .Select(x => new ListOfUserLanguage
                {
                    LanguageName = x.Languag.Name,
                    LanguageLevel = x.LanguageLevel,
                    Id = x.Id
                }).FirstOrDefaultAsync();

            return UserLanguage;
        }

        public async Task<(bool isSuccess, List<string> errors)> AddUserLanguage(AddUserLanguageDTO model)
        {
            var err = new List<string>();
            var language = new Domain.Languag();
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err);
                }
                var resomeId = user.ResomeId;

                language = await _Context.Languags.FindAsync(model.LanguageId);
                if (language == null)
                {
                    err.Add(" زبان  انتخابی شما یافت نشد");
                    return (false, err);
                }


                var newUserLanguage = new UserLanguage()
                {
                    LanguageId = language.Id,
                    LanguageLevel = model.LanguageLevel,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    ResomeId = resomeId.Value

                };


                await _Context.UserLanguage.AddAsync(newUserLanguage);
                await _Context.SaveChangesAsync();


                //resome
                //var resome = await _Context.Resomes.FindAsync(resomeId);
                //resome.UserLanguageId = newUserLanguage.Id;
                //_Context.Resomes.Update(resome);

                //await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddUserLanguage", "UserLanguage");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> DeleteUserLanguage(int id)
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
                var resomeId = user.ResomeId;
                var UserLanguage = await _Context.UserLanguage.
                    FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);
                if (UserLanguage == null)
                {
                    err.Add("زبان مورد نظر یافت نشد");
                    return (false, err);
                }
                _Context.UserLanguage.Remove(UserLanguage);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteUserLanguage", "UserLanguage");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }

        }




        public async Task<(bool isSuccess, List<string> errors)> UpdateUserLanguage(EditUserLanguageDTO model)
        {
            var err = new List<string>();
            var language = new Domain.Languag();
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err);
                }
                var resomeId = user.ResomeId;

                language = await _Context.Languags.FindAsync(model.LanguageId);
                if (language == null)
                {
                    err.Add("زبان انتخابی شما یافت نشد");
                    return (false, err);
                }

                var UserLanguage = await _Context.UserLanguage.
                    FirstOrDefaultAsync(x=>x.Id==model.Id&&x.ResomeId==resomeId);
                UserLanguage.UpdateDate = DateTime.Now;
                UserLanguage.LanguageId = model.LanguageId;
                UserLanguage.LanguageLevel = model.LanguageLevel;

                _Context.UserLanguage.Update(UserLanguage);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddUserLanguage", "UserLanguage");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }


    }
}
