using DNTPersianUtils.Core;
using Domain;
using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JobSkillService : IJobSkillService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;

        public JobSkillService(DataContext dataContext, IlogService ilog)
        {
            this._Context = dataContext;
            _ilog = ilog;
        }


        public async Task<JobSkillDetail> GetJobSkillById(int id)
        {
            var jobSkill = await _Context.JobSkills.Include(x => x.Category)
                .Select(x => new JobSkillDetail
                {
                    Id = x.Id,
                    CategoryId = x.Category.Id,
                    Name = x.Name,
                    CreatedDate = x.CreateDate.ToShortPersianDateTimeString(true)

                })
                .FirstOrDefaultAsync(x => x.Id == id);
            return jobSkill;
        }

        public async Task<List<ListOfJobSkill>> GetAllJobSkills()
        {
            var skilllist = _Context.JobSkills.Include(x => x.Category)
                .OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.IsActive)
               .Select(x => new ListOfJobSkill
               {
                   Id = x.Id,
                   Name = x.Name,
                   CategoryName = x.Category.Name,
                   IsActive = x.IsActive,
                   UpdateDate = x.UpdateDate.ToShortPersianDateTimeString(true)
               })
               .AsNoTracking().AsQueryable();
            var finalModel = await skilllist.ToListAsync();
            return finalModel;
        }
        public async Task<List<ListOfSkillForSelect>> GetAllJobSkillsForSelect()
        {
            var skilllist = _Context.JobSkills.Where(x => x.IsActive).Include(x => x.Category)
                          .Select(x => new ListOfSkillForSelect
                          {
                              Name = x.Name,
                              Id = x.Id
                          })
                          .AsNoTracking().AsQueryable();
            var finalModel = await skilllist.ToListAsync();
            return finalModel;
        }


        public async Task<(bool isSuccess, List<string> errors)> AddJobSkill(AddSkillDTO model)
        {
            var err = new List<string>();
            var category = new Category();
            try
            {

                //category = await _Context.Categories.FindAsync(model.CategoryId);
                //if (category == null)
                //{
                //    err.Add("حوزه فعالیت انتخابی شما یافت نشد");
                //    return (false, err);
                //}


                var newJobSkill = new JobSkill()
                {
                    Name = model.Name,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,

                };
                //if (category != null)
                //{
                //    newJobSkill.CategoryId = category.Id;
                //}
                newJobSkill.IsActive = true;
                await _Context.JobSkills.AddAsync(newJobSkill);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddJobSkill", "JobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> DisableJobSkill(int id)
        {
            var err = new List<string>();
            try
            {
                var jobskill = await _Context.JobSkills.FindAsync(id);
                if (jobskill == null)
                {
                    err.Add("مهارت مورد نظر یافت نشد");
                    return (false, err);
                }
                jobskill.IsActive = false;
                jobskill.UpdateDate = DateTime.Now;
                _Context.JobSkills.Update(jobskill);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteJobSkill", "JobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }

        }
        public async Task<(bool isSuccess, List<string> errors)> EnableJobSkill(int id)
        {
            var err = new List<string>();
            try
            {
                var jobskill = await _Context.JobSkills.FindAsync(id);
                if (jobskill == null)
                {
                    err.Add("مهارت مورد نظر یافت نشد");
                    return (false, err);
                }
                jobskill.IsActive = true;
                jobskill.UpdateDate = DateTime.Now;

                _Context.JobSkills.Update(jobskill);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteJobSkill", "JobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }

        }
        public async Task<(bool isSuccess, List<string> errors)> DeleteJobSkill(JobSkill model)
        {
            var err = new List<string>();
            try
            {
                if (model == null)
                {
                    err.Add("مهارت مورد نظر یافت نشد");
                    return (false, err);
                }
                _Context.JobSkills.Remove(model);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteJobSkill", "JobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }

        }



        public async Task<(bool isSuccess, List<string> errors)> UpdateJobSkill(EditSkillDTO model)
        {
            var err = new List<string>();
            var category = new Category();
            try
            {

                category = await _Context.Categories.FindAsync(model.CategoryId);
                if (category == null)
                {
                    err.Add("حوزه فعالیت انتخابی شما یافت نشد");
                    return (false, err);
                }

                var jobSkill = await _Context.JobSkills.FindAsync(model.Id);
                jobSkill.UpdateDate = DateTime.Now;
                jobSkill.Name = model.Name;

                if (category != null)
                {
                    jobSkill.CategoryId = category.Id;
                }

                _Context.JobSkills.Update(jobSkill);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddJobSkill", "JobSkill");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }


    }
}
