using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISettingService
    {
        Task<(bool isSuccess, string error)> CreateSetting(CreateSettingDTO model);
        Task<(bool isSuccess, string error)> EditSetting(EditSettingDTO model);
        Task<(bool isSuccess, string error)> ChnageIndexSetting(ChnageIndexSetting model);
        Task<(List<EditSettingDTO> model, bool isSuccess, string error)> GetAllSetting();
        Task<(List<EditSettingDTO> model, bool isSuccess, string error)> GetAllInfoForIndex();
        Task<(List<EditSettingDTO> model, bool isSuccess, string error)> GetAllInfoForAbouteUs();
        Task<(EditSettingDTO model, bool isSuccess, string error)> GetSettingById(int id);
        Task<(bool isSuccess, string error)> DeleteSetting(int id);
        Task<(bool isSuccess, string error)> CreateContactUs(ContactUsDTO model);
        Task<(bool isSuccess, string error, List<ContactUsDTO> model)> GetAllContactUs();
        Task<(bool isSuccess, string error, string content)> GetContactUsMessage(int id);
        Task<(bool isSuccess, string error, string content)> GetAboutUs();
        Task<(bool isSuccess, string error, string content)> GetKhadamatMa();
        Task<(bool isSuccess, string error, string content)> GetSharayetAkhzNamayande();



        Task<(bool isSuccess, string error, string content)> GetPolicy();
        Task<(bool isSuccess, string error, string content)> GetEmployeeHelper();


    }
}
