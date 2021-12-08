using Domain;
using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IResomeService
    {
        Task<ResomeCompeleteLevel> GetResomePercent(string? userId = null);
        Task<(bool isSuccess,string error)> SetResomeFile(UploadResomePDF PdfFile);
        Task<(bool isSuccess,string error)> LoadResomePDF();
        Task<(bool isSuccess,string error)> DeleteResomePDF();
        Task<(bool isSuccess,string error)> AddAboutMe(string aboutMe);
        Task<(bool isSuccess,string error)> LoadAboutMe();
        Task<(bool isSuccess,string error)> AsignResomeToAdver(int adverId);
        Task<(bool isSuccess, string error)> AsignResomeToListOFAdvers(List<int> adversId);

        Task<(bool isSuccess,string error)> MarkedAsignResome(int asignId);
        Task<(bool isSuccess,string error)> UnMarkedAsignResome(int asignId);
        Task<(bool isSuccess,string error)> AddCommentForAsignResome(AddCommentForResome model);
        Task<(bool isSuccess,string error)> DeleteCommentForAsignResome(int asignId); 
        Task<(string comment,string error)> LoadCommentForAsignResome(int asignId);
        Task<List<GetAllResomesInfoForAdver>> GetAllResomesInfoForAdver(int adverId);
        Task<List<GetAllResomesInfoForAdver>> FilterAllResomesInfoForAdver(FilterAsingResome model);
        Task<GetAllResomesInfoWitCount> GetAllResomesInfoWitCount(int adverId);
        Task<(UserShortInfoForResome result, bool isSuccess, string error)> GetUserShortInfoForResome(int asignId); 
        Task<(UserAllInfoForResome result, bool isSuccess, string error)> UserAllInfoForResome(int resomeId);

        Task<(bool isSuccess, string error)> ChangeAsignResomeStatus(ChangeAsignResomeStatus model);
        Task<(bool isSuccess, string error, AsingResomeStatus? model)> GetAsignResomeStatus( int asignResomeId);


        Task<(bool isSuccess, string error, List<MyAsignListDTO> model)> GetAllAsignResomeForEmployee();
        Task<(bool isSuccess, string error, ResomeAsignDetail model)> GetAsignResomeDetailsForEmployee(int asignId);


    }
}
