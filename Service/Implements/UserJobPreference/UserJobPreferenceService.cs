using AutoMapper;
using DNTPersianUtils.Core;
using Domain;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserJobPreferenceService : IUserJobPreferenceService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public UserJobPreferenceService(DataContext dataContext, IlogService ilog,
            IMapper mapper, IAccountService accountService)
        {
            this._Context = dataContext;
            _ilog = ilog;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ListOfUserJobPreferences> GetUserJobPreferenceForCurrentUser()
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserJobPreference =await _Context.UserJobPreferences.Include(x => x.UserJobPreferenceCategories).ThenInclude(x=>x.Category)
                .Where(x => x.ResomeId == resomeId)
                          .AsNoTracking().AsQueryable().FirstOrDefaultAsync();
            if (UserJobPreference == null) return null; 
            var catList = new List<CategoryForJobPrefence>();
            foreach (var item in  UserJobPreference.UserJobPreferenceCategories)
            {
                catList.Add(new CategoryForJobPrefence() { CategoryId = item.CategoryId, CategoryName = item.Category.Name });
            };
            var finalModel = _mapper.Map<UserJobPreferences, ListOfUserJobPreferences>( UserJobPreference);
            finalModel.CategoryForJobPrefence = catList;

            return finalModel;
        }



        public async Task<UserJobPreferences> GetUserJobPreferenceById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserJobPreference = await _Context.UserJobPreferences/*.Include(x => x.Category)*/
                .FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);
            return UserJobPreference;
        }

        public async Task<ListOfUserJobPreferences> GetUserJobPreferenceDetailsById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserJobPreference = await _Context.UserJobPreferences.Include(c => c.UserJobPreferenceCategories)
                .FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);


            var finalModel = _mapper.Map<UserJobPreferences, ListOfUserJobPreferences>(UserJobPreference);
            finalModel.CategoryForJobPrefence = new List<CategoryForJobPrefence>();

            foreach (var catId in UserJobPreference.UserJobPreferenceCategories)
            {
                var category = await _Context.Categories.FindAsync(catId.CategoryId);
                finalModel.CategoryForJobPrefence.Add(new CategoryForJobPrefence()
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name
                });

            }
            return finalModel;
        }

        public async Task<(bool isSuccess, List<string> errors)> AddUserJobPreference(AddUserJobPreferencesDTO model)
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

                foreach (var item in model.CategoryIds)
                {
                    if (!await _Context.Categories.AnyAsync(x => x.Id == item))
                    {
                        model.CategoryIds.Remove(item);
                    }
                }

                var newUserJobPreference = _mapper.Map<AddUserJobPreferencesDTO, UserJobPreferences>(model);


                newUserJobPreference.CreateDate = DateTime.Now;
                newUserJobPreference.UpdateDate = DateTime.Now;
                newUserJobPreference.ResomeId = resomeId.Value;


                await _Context.UserJobPreferences.AddAsync(newUserJobPreference);
                await _Context.SaveChangesAsync();


                //resome
                var resome = await _Context.Resomes.FindAsync(resomeId);
                resome.UserJobPreferencesId = newUserJobPreference.Id;
                _Context.Resomes.Update(resome);


                await _Context.SaveChangesAsync();

                var listOfUserPrefrenceCategory = new List<UserJobPreferenceCategory>();
                foreach (var item in model.CategoryIds)
                {
                    var newUserPrefrenceCategory = new UserJobPreferenceCategory()
                    {
                        CategoryId = item,
                        UserJobPreferenceId = newUserJobPreference.Id
                    };
                    listOfUserPrefrenceCategory.Add(newUserPrefrenceCategory);
                }
                await _Context.UserJobPreferenceCategories.AddRangeAsync(listOfUserPrefrenceCategory);
                await _Context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddUserJobPreference", "UserJobPreference");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> DeleteUserJobPreference(int id)
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
                var UserJobPreference = await _Context.UserJobPreferences.
                    FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId.Value);
                if (UserJobPreference == null)
                {
                    err.Add("مهارت مورد نظر یافت نشد");
                    return (false, err);
                }
                _Context.UserJobPreferences.Remove(UserJobPreference);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteUserJobPreference", "UserJobPreference");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }

        }




        public async Task<(bool isSuccess, List<string> errors)> UpdateUserJobPreference(EditUserJobPreferencesDTO model)
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
                var userJobPreference = await _Context.UserJobPreferences.Include(c => c.UserJobPreferenceCategories)
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.ResomeId == resomeId);



                var deletedItems = userJobPreference.UserJobPreferenceCategories;




                var notValidCatId = new List<int>();
                foreach (var item in model.CategoryIds)
                {
                    if (!await _Context.Categories.AnyAsync(x => x.Id == item))
                    {
                        notValidCatId.Add(item);
                    }
                }
                foreach (var item in notValidCatId)
                {
                    model.CategoryIds.Remove(item);
                }
                userJobPreference.UpdateDate = DateTime.Now;
                userJobPreference.City = model.City;
                userJobPreference.TypeOfCooperation = model.TypeOfCooperation;
                userJobPreference.Senioritylevel = model.Senioritylevel;
                userJobPreference.Salary = model.Salary;
                userJobPreference.Insurance = model.Insurance;
                userJobPreference.EducationCourses = model.EducationCourses;
                userJobPreference.Promotion = model.Promotion;
                userJobPreference.FlexibleWorkingTime = model.FlexibleWorkingTime;
                userJobPreference.HasMeel = model.HasMeel;
                userJobPreference.TransportationService = model.TransportationService;

                _Context.UserJobPreferences.Update(userJobPreference);
                //_Context.Entry(userJobPreference).State = EntityState.Modified;
                if (deletedItems.Count() > 0)
                {
                    _Context.UserJobPreferenceCategories.RemoveRange(deletedItems);
                }

                var listOfUserPrefrenceCategory = new List<UserJobPreferenceCategory>();
                foreach (var item in model.CategoryIds)
                {
                    var newUserPrefrenceCategory = new UserJobPreferenceCategory()
                    {
                        CategoryId = item,
                        UserJobPreferenceId = userJobPreference.Id
                    };
                    listOfUserPrefrenceCategory.Add(newUserPrefrenceCategory);
                }
                await _Context.UserJobPreferenceCategories.AddRangeAsync(listOfUserPrefrenceCategory);
                await _Context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddUserJobPreference", "UserJobPreference");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }


    }
}
