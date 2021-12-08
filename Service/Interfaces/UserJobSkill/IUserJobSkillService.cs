using Domain;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserJobSkillService

    {
        Task<List<ListOfUserJobSkill>> GetAllUserJobSkillForCurrentUser();
        Task<UserJobSkill> GetUserJobSkillById(int id);
        Task<ListOfUserJobSkill> GetUserJobSkillDetailsById(int id);
        Task<(bool isSuccess, List<string> errors,int result)> AddUserJobSkill(AddUserJobSkillDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateUserJobSkill(EditUserJobSkillDTO model);
        Task<(bool isSuccess, List<string> errors)> DeleteUserJobSkill(int id);
    }
}
