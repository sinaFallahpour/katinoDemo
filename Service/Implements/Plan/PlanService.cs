using AutoMapper;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
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
    public class PlanService : IPlanService
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IlogService _ilog;

        public PlanService(DataContext dataContext, IAccountService accountService
            , IMapper mapper, IlogService ilog)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _mapper = mapper;
            _ilog = ilog;
        }
        public async Task<(bool isSuccess, List<string> errors)> CreatePlan(CreatePlanDTO model)
        {
            try
            {
                var plan = _mapper.Map<CreatePlanDTO, Plan>(model);
                plan.IsActive = true;
                plan.CreateAt = DateTime.Now;
                plan.UpdataAt = DateTime.Now;
                plan.IsActive = true;
                plan.IsFree = model.IsFree;

                await _dataContext.Plans.AddAsync(plan);
                await _dataContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CreatePlan", "Plan");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }


        public async Task<(bool isSuccess, List<string> errors)> EditPlan(EditPlanDTO model)
        {
            try
            {
                var plan = _mapper.Map<EditPlanDTO, Plan>(model);
                plan.UpdataAt = DateTime.Now;
                plan.IsActive = true;
                plan.IsFree = model.IsFree;
                _dataContext.Plans.Update(plan);
                await _dataContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditPlan", "Plan");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);
            }
        }
        public async Task<(bool isSuccess, List<string> errors)> DisablePlan(int planId)
        {
            try
            {
                var plan = await _dataContext.Plans.FindAsync(planId);
                plan.UpdataAt = DateTime.Now;
                plan.IsActive = false;
                _dataContext.Plans.Update(plan);
                await _dataContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DisablePlan", "Plan");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> EnablePlan(int planId)
        {
            try
            {
                var plan = await _dataContext.Plans.FindAsync(planId);
                plan.UpdataAt = DateTime.Now;
                plan.IsActive = true;
                _dataContext.Plans.Update(plan);
                await _dataContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EnablePlan", "Plan");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<AllResult<List<AllPlanForAdmin>>> GetAllPlanForAdmin(int page = 0, int pageSize = PublicHelper.PageSize)
        {
            if (page == 0)
            {
                page = 1;
            }

            var plans = _dataContext.Plans.AsQueryable().AsNoTracking();
            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(plans.Count()) / Convert.ToDouble(pageSize)));
            var plansList = await plans
                .OrderByDescending(o => o.Id)
                 .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            var finalmodel = _mapper.Map<List<Plan>, List<AllPlanForAdmin>>(plansList);

            return new AllResult<List<AllPlanForAdmin>>()
            {
                PageCount = pageCount,
                ListOfData = finalmodel
            };

        }

        public async Task<List<AllPlanForCompany>> GetAllPlanForCompany()
        {
            var plans = await _dataContext.Plans.Where(x => x.IsActive)
                .OrderByDescending(c => c.priority)
                .ThenByDescending(x => x.UpdataAt)
                .ToListAsync();
            var finalModel = _mapper.Map<List<Plan>, List<AllPlanForCompany>>(plans);
            return finalModel;
        }

        public async Task<LoadPlan> LoadPlan(int planId)
        {

            var plan = await _dataContext.Plans.FindAsync(planId);
            var planModel = _mapper.Map<Plan, LoadPlan>(plan);
            return planModel;
        }

        public async Task<PlanInfo> PlanInfoForCompany()
        {
            var userUsePlanCount = 0;
            var userUsePlanForImmediateAdverCount = 0;
            var remainingDate = 0.0;

            var planInfo = new PlanInfo()
            {
                PlanName = "",
                RemainingAdversCount = "0",
                RemainingImmediateAdversCount = "0",
                RemainingDays = "0",
                PlanAdverCount = "0",
                PlanImmediateAdverCount = "0",
                PlanStoryAdverCount = "0"
            };
            var company = await _accountService.GetCurrectUser();
            var userPlan = await _dataContext.Plans.FindAsync(company.PlanId);

            if (company == null)
            {
                return planInfo;
            }

            if (company.PlanId == null)
            {
                return planInfo;
            }
            var userAdvers = _dataContext.JobAdvertisements
                  .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                  .AsQueryable().AsNoTracking();

            userUsePlanCount = userAdvers.Count();
            userUsePlanForImmediateAdverCount = userAdvers.Where(i => i.IsImmediate).Count();

            var userShopThisPlan = _dataContext.Factors
                        .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                .AsQueryable().AsNoTracking().Count();
            //get user shops count for this plan in facto and get adver count in plan info and difrent of that and user use adver in adver table meaning reaminadvercount for this user
            var remainAdverCount = (userShopThisPlan * userPlan.AdverCount) - userUsePlanCount;
            var remainImmediateAdverCount = (userShopThisPlan * userPlan.ImmediateAdverCount) - userUsePlanForImmediateAdverCount;

            if (remainAdverCount <= 0)
            {
                planInfo.RemainingAdversCount = "0";
            }
            if (remainImmediateAdverCount <= 0)
            {
                var userShopImmediatelyInFactor = _dataContext.Factors
                           .Where(x => x.IsImmediately && x.CompanyId == company.Id)
                           .Count();
                remainImmediateAdverCount = ((userShopThisPlan * userPlan.ImmediateAdverCount) + userShopImmediatelyInFactor) - userUsePlanForImmediateAdverCount;

                planInfo.RemainingImmediateAdversCount = "0";
            }
            var userLastShopThisPlan = await _dataContext.Factors
                .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                .Select(x => x.Date)
                .OrderBy(x => x.Date)
                .LastOrDefaultAsync();

            remainingDate = (userLastShopThisPlan.AddDays(userPlan.Duration) - DateTime.Now).TotalDays;
            if (remainingDate <= 0)
            {
                planInfo.RemainingDays = "0";

            }
            planInfo.RemainingDays = remainingDate.ToString();
            planInfo.RemainingAdversCount = remainAdverCount.ToString();
            planInfo.RemainingImmediateAdversCount = remainImmediateAdverCount.ToString();
            planInfo.RemainingAdversCount /*.RemainingImmediateAdversCount */= remainImmediateAdverCount.ToString();
            planInfo.PlanName = userPlan.Title;
            planInfo.PlanAdverCount = userPlan.AdverCount.ToString();
            planInfo.PlanImmediateAdverCount = userPlan.ImmediateAdverCount.ToString();
            planInfo.PlanStoryAdverCount = userPlan.StoryCount.ToString();


            return planInfo;

        }

        public async Task<(bool isSuccess, List<string> errors)> ShopPlan(int planId)
        {
            var error = new List<string>();
            var company = await _accountService.GetCurrectUser();
            if (company == null)
            {
                error.Add("کاربر نامعتبر");
                return (false, error);
            }
            var plan = await _dataContext.Plans.FindAsync(planId);
            //check transaction
            var factor = new Factor()
            {
                CompanyId = company.Id,
                PlanId = planId,
                Price = plan.Price,
                Date = DateTime.Now,
                IsImmediately = false,
            };


            _dataContext.Users.Update(company);
            await _dataContext.SaveChangesAsync();
            return (true, null);


        }

        public async Task<AllPlanForEmployee> GetAllPlanForEmployee()
        {
            var plan = await _dataContext.EmployeePlans
                            .Select(x => new AllPlanForEmployee
                            {
                                Id = x.Id,
                                AdverCount = "بی نهایت",
                                Duration = "یکسال",
                                Price = x.Price,
                                Title = x.Title,
                            })
                            .FirstOrDefaultAsync();
            return plan;
        }
    }
}
