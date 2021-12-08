using Domain;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ILanguageService
    {
        Task<List<ListOfLanguagesForSelect>> GetAllLanguags();
        Task<List<ListOfLanguagesForAdmin>> GetAllLanguagsForAdmin();
        Task<Languag> GetLanguagById(int id);
        Task<(bool isSuccess, List<string> errors)> AddLanguag(AddLanguageDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateLanguag(UpdateLanguageDTO model);
        Task<(bool isSuccess, List<string> errors)> DisableLanguag(int id);
        Task<(bool isSuccess, List<string> errors)> EnableLanguag(int id);
    }
}
