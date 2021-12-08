using AutoMapper;
using DNTPersianUtils.Core;
using Domain;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.Interfaces.Account;
using Service.Interfaces.UserWorkExperience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service
{
    public class UserWorkExperienceService : IUserWorkExperienceService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public UserWorkExperienceService(DataContext dataContext, IlogService ilog,
            IMapper mapper, IAccountService accountService)
        {
            _Context = dataContext;
            _ilog = ilog;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<(bool isSuccess, List<string> errors)> AddWorkExperience(UserWorkExperienceDTO model)
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
                var finalModel = _mapper.Map<UserWorkExperienceDTO, UserWorkExperience>(model);
                //if (!string.IsNullOrEmpty(model.StartDate))
                //{
                //    finalModel.StartDate = model.StartDate.ToGeorgianDateTime();
                //}
                //if (!string.IsNullOrEmpty(model.EndDate))
                //{
                //    finalModel.EndDate = model.StartDate.ToGeorgianDateTime();
                //}
                finalModel.CreateDate = DateTime.Now;
                finalModel.UpdateDate = DateTime.Now;
                finalModel.IsActive = true;
                finalModel.ResomeId = resomeId.Value;
                await _Context.UserWorkExperiences.AddAsync(finalModel);
                await _Context.SaveChangesAsync();


                //resome
                //var resome = await _Context.Resomes.FindAsync(resomeId);
                //resome.UserWorkExperienceId = finalModel.Id;
                //_Context.Resomes.Update(resome);
                //await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddWorkExperience", "UserWorkExperience");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> DeleteWorkExperience(int id)
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
                var deletedModel = await _Context.UserWorkExperiences.
                    FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);
                deletedModel.IsActive = false;
                _Context.UserWorkExperiences.Update(deletedModel);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteWorkExperience", "UserWorkExperience");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<List<UserWorkExperienceDTO>> GetAllWorkExperienceForCuurectUser()
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            //var user = _accountService.GetCurrectUser();
            var userWorkExperiences = _Context.UserWorkExperiences/*.Where(check user)*/
                .Where(x => x.IsActive && x.ResomeId == resomeId)
                .AsNoTracking().AsQueryable();

            var userWorkExperiencesList = await userWorkExperiences.ToListAsync();
            var finalModel = _mapper.Map<List<UserWorkExperience>, List<UserWorkExperienceDTO>>(userWorkExperiencesList);

            return finalModel;

        }

        public async Task<UserWorkExperienceDTO> GetWorkExperienceById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var WorkExperienc = await _Context.UserWorkExperiences.
                FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);

            var finalModel = _mapper.Map<UserWorkExperience, UserWorkExperienceDTO>(WorkExperienc);

            return finalModel;
        }

        public async Task<(bool isSuccess, List<string> errors)> UpdateWorkExperience(UpdateUserWorkExperienceDTO model)
        {
            var err = new List<string>();

            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive && !user.IsActive)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err);
                }
                var resomeId = user.ResomeId;
                var finalModel = _mapper.Map<UpdateUserWorkExperienceDTO, UserWorkExperience>(model);
                if(resomeId.HasValue) finalModel.ResomeId = resomeId.Value;

                finalModel.IsActive = true;

                _Context.UserWorkExperiences.Update(finalModel);
                finalModel.UpdateDate = DateTime.Now;
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UpdateWorkExperience", "UserWorkExperience");
                err.Add(JsonConvert.SerializeObject(ex));
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
    }
}