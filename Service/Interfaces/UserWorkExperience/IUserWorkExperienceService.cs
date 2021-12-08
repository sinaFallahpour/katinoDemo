using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.UserWorkExperience
{
    public interface IUserWorkExperienceService
    {
        Task<List<UserWorkExperienceDTO>> GetAllWorkExperienceForCuurectUser();
        Task<UserWorkExperienceDTO> GetWorkExperienceById(int id);
        Task<(bool isSuccess, List<string> errors)> AddWorkExperience(UserWorkExperienceDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateWorkExperience(UpdateUserWorkExperienceDTO model);
        Task<(bool isSuccess, List<string> errors)> DeleteWorkExperience(int id);
    }
}
