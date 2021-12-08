using AutoMapper;
using DNTPersianUtils.Core;
using Domain;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EducationalBackgroundServic : IEducationalBackgroundService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public EducationalBackgroundServic(DataContext dataContext, IlogService ilog,
            IMapper mapper, IAccountService accountService)
        {
            _Context = dataContext;
            _ilog = ilog;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<(bool isSuccess, List<string> errors)> AddEducationalBackground(EducationalBackgroundDTO model)
        {
            var err = new List<string>();

            try
            {
                var user =await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err);
                }
                var resomeId = user.ResomeId;
                var finalModel = _mapper.Map<EducationalBackgroundDTO, EducationalBackground>(model);
                finalModel.CreateDate = DateTime.Now;
                finalModel.UpdateDate = DateTime.Now;
                finalModel.ResomeId = resomeId.Value;
                finalModel.IsActive = true;
                await _Context.EducationalBackgrounds.AddAsync(finalModel);
                await _Context.SaveChangesAsync();


                //resome
                //var resome =await _Context.Resomes.FindAsync(resomeId);
                //resome.EducationalBackgroundId = finalModel.Id;
                //_Context.Resomes.Update(resome);
                //await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddEducationalBackground", " EducationalBackground");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> DeleteEducationalBackground(int id)
        {

            try
            {
                var deletedModel = await _Context.EducationalBackgrounds.FindAsync(id);
                deletedModel.IsActive = false;
                _Context.EducationalBackgrounds.Update(deletedModel);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteEducationalBackground", " EducationalBackground");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<List<UpdateEducationalBackgroundDTO>> GetAllEducationalBackgroundForCuurectUser()
        {

            var user =await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                return null;
                }
                var resomeId = user.ResomeId;
            var EducationalBackgrounds = _Context.EducationalBackgrounds/*.Where(check user)*/
                .Where(x => x.IsActive&&x.ResomeId==resomeId)
                .AsNoTracking().AsQueryable();

            var EducationalBackgroundsList = await EducationalBackgrounds.ToListAsync();
            var finalModel = _mapper.Map<List<EducationalBackground>, List<UpdateEducationalBackgroundDTO>>(EducationalBackgroundsList);

            return finalModel;

        }

        public async Task<UpdateEducationalBackgroundDTO> GetEducationalBackgroundById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var WorkExperienc = await _Context.EducationalBackgrounds
                .FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);

            var finalModel = _mapper.Map<EducationalBackground, UpdateEducationalBackgroundDTO>(WorkExperienc);

            return finalModel;
        }

        public async Task<(bool isSuccess, List<string> errors)> UpdateEducationalBackground(UpdateEducationalBackgroundDTO model)
        {
            var err = new List<string>();

            try
            {
                var user = await _accountService.GetCurrectUser();

                if (user == null||!user.ResomeId.HasValue)
                {
                    err.Add("کاربر نامعتبر");
                    return (false, err);
                }
                var finalModel = _mapper.Map<UpdateEducationalBackgroundDTO, EducationalBackground>(model);

                _Context.EducationalBackgrounds.Update(finalModel);
                finalModel.UpdateDate = DateTime.Now;
                finalModel.ResomeId = user.ResomeId.Value;
                finalModel.IsActive = true;
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UpdateEducationalBackground", " EducationalBackground");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
    }
}
