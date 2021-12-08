using Domain;
using Domain.DTO.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.Account
{
    public interface IAccountService
    {
        Task<User> FindUserByPhonenumber(string PhoneNumber);
        Task<User> FindUserById(string id);


        Task<(int status, List<string> erros)> CheckVeyficatioCode(VerifyDTO model);

        Task<(bool isSuccess, string message, string mobile)> RefrenceRegister(RefrenceRegisterDTO model);

        Task<SignInResult> CheckPasswordAsync(User User, string Password);



        //generate password resete token
        Task<string> GeneratePasswordResetToken(User user);





        /// check Token paload(serialNUmber) Is valid
        Task<bool> CheckTokenIsValid();
        Task<User> GetCurrectUser();
        Task<bool> CheckUserIsActive(string id);
        Task<User> CheckIsCurrentUserName(string id);

        Task<List<ListOfCompanies>> ListOfCompanies();
        Task<List<ListOfCompanies>> SearchInCompanies(string key);
        Task<List<ListOfCompanies>> ListOfTop50Companies();
        Task<CompanyDetails> CompanyDetails(string CompanyEngName);

        Task<List<ListOfEmployees>> ListOfEmployees();
        //block unblock
        Task<(bool isSuccess, List<string> message)> BlockUser(string userId);
        Task<(bool isSuccess, List<string> message)> UnblockUser(string userId);

        Task<GetEmployerInfo> EmployerInfo();
        Task<(bool isSuccess, List<string> message)> EditEmployerInfo(EmployerInfo model);
        Task<(bool isSuccess, string message)> GetUserRole();

        Task<List<CompanyLogoInIndex>> GetCompanyLogoInIndex();

        Task<(bool isSuccess, List<string> message)> EditEmployeePersonalInformation(EmployeePersonalInformationDTO model);
        Task<LoadEmployeePersonalInformationDTO> GetEmployeePersonalInformation();
        Task<List<AllCompaniesForAdminDTO>> GetAllComapinesForAdmin(string RefrenceId = "");
        Task<List<AllEmployeeForAdmin>> GetAllEmployeeForAdmin(string RefrenceId = "");
        Task<List<AllEmployeeForAdmin>> GetAllRefrenceForAdmin(bool isAdmin,string RefrenceId = "");
        //Task<LoadEmployeePersonalInformationDTO> GetAllEmployeeForAdmin();
        Task<(bool isSuccess, string message)> EditShebaCode(string shebaCode);
        Task<(bool isSuccess, string message, string shebaCode)> LoadShebaCode();

        Task<(bool isSuccess, string message)> RatingCompany(int rate, string userId);
        Task<(bool isSuccess, string message, CitiesDTO model)> GetCityForTicket();

        Task<(bool isSucces, string message, RefrenceDashboard model)> GetRefrenceDashboardInfo();
        ResomeCompeleteLevel GetResomePercent(List<Resome> resomes, string userId);








    }
}
