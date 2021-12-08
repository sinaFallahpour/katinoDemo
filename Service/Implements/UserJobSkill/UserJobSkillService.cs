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
    public class UserJobSkillService : IUserJobSkillService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;
        private readonly IAccountService _accountService;
        public UserJobSkillService(DataContext dataContext, IlogService ilog, IAccountService accountService)
        {
            this._Context = dataContext;
            _ilog = ilog;
            _accountService = accountService;
        }


        public async Task<List<ListOfUserJobSkill>> GetAllUserJobSkillForCurrentUser()
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserJobSkilllist = _Context.UserJobSkills.Include(x => x.JobSkill)
                          .Where(x => x.ResomeId == resomeId)
                          .Select(x => new ListOfUserJobSkill
                          {
                              Id = x.Id,
                              JobSkillName = x.JobSkill.Name
                          })
                          .AsNoTracking().AsQueryable();

            var finalModel = await UserJobSkilllist.ToListAsync();
            return finalModel;
        }



        public async Task<UserJobSkill> GetUserJobSkillById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserJobSkill = await _Context.UserJobSkills.Include(x => x.JobSkill)
                .FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);
            return UserJobSkill;
        }

        public async Task<ListOfUserJobSkill> GetUserJobSkillDetailsById(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null && !user.IsActive)
            {
                return null;
            }
            var resomeId = user.ResomeId;
            var UserJobSkill = await _Context.UserJobSkills.Include(x => x.JobSkill)
                .Where(x => x.Id == id && x.ResomeId == resomeId)
                .Select(x => new ListOfUserJobSkill
                {
                    Id = x.Id,
                    JobSkillName = x.JobSkill.Name
                }).FirstOrDefaultAsync();

            return UserJobSkill;
        }

        public async Task<(bool isSuccess, List<string> errors,int result)> AddUserJobSkill(AddUserJobSkillDTO model)
        {
            var err = new List<string>();
            var jobSkill = new JobSkill();
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err,0);
                }
                var resomeId = user.ResomeId;

                jobSkill = await _Context.JobSkills.FindAsync(model.JobSkillId);
                if (jobSkill == null)
                {
                    err.Add(" مهارت  انتخابی شما یافت نشد");
                    return (false, err,0);
                }

                if(await _Context.UserJobSkills.AnyAsync(x => x.ResomeId == resomeId && x.JobSkillId == jobSkill.Id))
                {
                    err.Add(" انتخاب شده است");
                    return (false, err, 0);
                }
                var newUserJobSkill = new UserJobSkill()
                {
                    JobSkillId = model.JobSkillId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    ResomeId = resomeId.Value

                };


                await _Context.UserJobSkills.AddAsync(newUserJobSkill);
                await _Context.SaveChangesAsync();


                //resome
                //var resome = await _Context.Resomes.FindAsync(resomeId);
                //resome.UserJobSkillId = newUserJobSkill.Id;
                //_Context.Resomes.Update(resome);


                //await _Context.SaveChangesAsync();
                return (true, null,newUserJobSkill.Id);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddUserJobSkill", "UserJobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err,0);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> DeleteUserJobSkill(int id)
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
                var UserJobSkill = await _Context.UserJobSkills
                    .FirstOrDefaultAsync(x => x.Id == id && x.ResomeId == resomeId);
                if (UserJobSkill == null)
                {
                    err.Add("مهارت مورد نظر یافت نشد");
                    return (false, err);
                }
                _Context.UserJobSkills.Remove(UserJobSkill);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteUserJobSkill", "UserJobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }

        }




        public async Task<(bool isSuccess, List<string> errors)> UpdateUserJobSkill(EditUserJobSkillDTO model)
        {
            var err = new List<string>();
            var jobSkill = new JobSkill();
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null && !user.IsActive)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err);
                }
                var resomeId = user.ResomeId;

                jobSkill = await _Context.JobSkills.FindAsync(model.JobSkillId);
                if (jobSkill == null)
                {
                    err.Add("مهارت انتخابی شما یافت نشد");
                    return (false, err);
                }

                var UserJobSkill = await _Context.UserJobSkills.FirstOrDefaultAsync
                    (x => x.Id == model.Id && x.ResomeId == resomeId);
                UserJobSkill.UpdateDate = DateTime.Now;
                UserJobSkill.JobSkillId = model.JobSkillId;

                _Context.UserJobSkills.Update(UserJobSkill);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddUserJobSkill", "UserJobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }


    }
}
