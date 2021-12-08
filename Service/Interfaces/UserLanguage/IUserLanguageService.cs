using Domain;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserLanguageService

    {
        Task<List<ListOfUserLanguage>> GetAllUserLanguagesForCurrentUser();
        Task<ListOfUserLanguage> GetUserLanguageById(int id);
        Task<ListOfUserLanguage> GetUserLanguageDetailsById(int id);
        Task<(bool isSuccess, List<string> errors)> AddUserLanguage(AddUserLanguageDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateUserLanguage(EditUserLanguageDTO model);
        Task<(bool isSuccess, List<string> errors)> DeleteUserLanguage(int id);
    }
}
