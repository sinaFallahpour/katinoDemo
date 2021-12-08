using Domain;
using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPlanService

    {
        Task<(bool isSuccess, List<string> errors)> CreatePlan(CreatePlanDTO model);
        Task<(bool isSuccess, List<string> errors)> EditPlan(EditPlanDTO model);
        Task<LoadPlan> LoadPlan(int planId);
        Task<(bool isSuccess, List<string> errors)> EnablePlan(int planId);
        Task<(bool isSuccess, List<string> errors)> DisablePlan(int planId);

        Task<AllResult<List<AllPlanForAdmin>>> GetAllPlanForAdmin(int page = 0, int pageSize = PublicHelper.PageSize);
        Task<List<AllPlanForCompany>> GetAllPlanForCompany();
        Task<AllPlanForEmployee> GetAllPlanForEmployee();

        Task<PlanInfo> PlanInfoForCompany();




    }
}
