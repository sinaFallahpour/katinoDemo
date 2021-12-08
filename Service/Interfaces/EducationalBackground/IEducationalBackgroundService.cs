using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEducationalBackgroundService
    {
        Task<List<UpdateEducationalBackgroundDTO>> GetAllEducationalBackgroundForCuurectUser();
        Task<UpdateEducationalBackgroundDTO> GetEducationalBackgroundById(int id);
        Task<(bool isSuccess, List<string> errors)> AddEducationalBackground(EducationalBackgroundDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateEducationalBackground(UpdateEducationalBackgroundDTO model);
        Task<(bool isSuccess, List<string> errors)> DeleteEducationalBackground(int id);
    }
}
