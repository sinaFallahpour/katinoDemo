using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IJobOpportunityService
    {
        Task<(bool isSuccess, List<string> errors)> CreateJobOpportunity(CreateJobOpportunity model);
        Task GetSeggustAdverForUser();



    }
}
