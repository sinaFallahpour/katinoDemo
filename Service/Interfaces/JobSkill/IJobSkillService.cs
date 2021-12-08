using Domain;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IJobSkillService

    {
        Task<List<ListOfJobSkill>> GetAllJobSkills();
        Task<List<ListOfSkillForSelect>> GetAllJobSkillsForSelect();
        Task<JobSkillDetail> GetJobSkillById(int id);
        Task<(bool isSuccess, List<string> errors)> AddJobSkill(AddSkillDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateJobSkill(EditSkillDTO model);
        Task<(bool isSuccess, List<string> errors)> EnableJobSkill(int id);
        Task<(bool isSuccess, List<string> errors)> DisableJobSkill(int id);
    }
}
