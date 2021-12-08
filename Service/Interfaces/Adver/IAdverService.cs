using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAdverService
    {

        Task<JobAdvertisement> GetAdverById(int id);
        Task<LoadAdverDTO> LoadAdver(int id);
        Task<AdverDetail> AdverDetails(int id);


        Task<AllResult<List<AllAdverForAdmin>>> GetAllAdverByFilteringForAdmin(bool? isImmediatley, AdverStatus? adverStatus, AdverCreatationStatus? adverCreatationStatus, int page,int adverCities, int pageSize = PublicHelper.PageSize, string RefrenceId = "");
        Task<List<AllAdverForCurrectUser>> GetAllAdverByStatusForCurrectUser(AdverStatus adverStatus);
        Task<List<AllAdverForCurrectUser>> SearchAdverForCurrectUser(string key);
        Task<AllResult<List<AllAdverForAdmin>>> GetAllAdverForAdmin(int page, int pageSize = PublicHelper.PageSize, string RefrenceId = "");
        Task<List<AllAdverForCurrectUser>> GetAllAdverForCurrectUser();
        Task<AllResult<List<AllAdver>>> GetAllAdverForIndex(int page, int pageSize = PublicHelper.PageSize);
        Task<AllResult<List<AllAdver>>> GetAllMarkedAdverForUser(int page, int pageSize = PublicHelper.PageSize);
        Task<AllResult<List<AllAdver>>> GetLastAdversForIndex(int page, int pageSize = PublicHelper.PageSize);
        Task<AllResult<List<AllAdver>>> GetLastImmediatelyAdversForIndex(int page, int pageSize = PublicHelper.PageSize);
        Task<AllResult<List<AllAdver>>> SearchAdver(string key, string city, int page, int pageSize = PublicHelper.PageSize);
        Task<AllResult<List<AllAdver>>> FilterAdver(FilterAdverDTO model, int page, int pageSize = PublicHelper.PageSize);
        Task<AllResult<List<AllAdver>>> SuggestionAdverForUser(int page, int pageSize = PublicHelper.PageSize);

        Task<List<AllAdver>> TheSameAdver(int adverId);


        Task<AdverStatus> GetAdverStatus(int id);
        Task<bool> IsActiveAdver(int id);
        Task<(bool isSuccess, List<string> errors)> SetAddverImmediate(int adverId);

        Task<(bool isSuccess, List<string> errors)> AddAdver(AddAdverDTO model);
        Task<(bool isSuccess, List<string> errors)> SaveToDraft(AddAdverDTO model);
        Task<(bool isSuccess, List<string> errors)> EditAdver(EditAdverDTO model);
        Task<(bool isSuccess, List<string> errors)> AddAdverFromDraft(int adverId);

        Task<(bool isSuccess, List<string> errors)> FinishedAdver(int id);
        Task<(bool isSuccess, List<string> errors)> DisableAdver(int id);
        Task<(bool isSuccess, List<string> errors)> ArchiveAdver(int id);
        Task<(bool isSuccess, List<string> errors)> EnableAdver(int id);//enbale adver from draft
        Task<(bool isSuccess, List<string> errors)> EnableAgainAdver(int id);//add Date from 2 month when expired


        //check user can create Adver
        Task<(bool isSuccess, List<string> errors)> CanCreateAdver(User company, bool isImmediate);
        Task<(bool isSuccess, string error)> MarkAdver(int adverId);
        Task<(bool isSuccess, string error)> UnMarkAdver(int adverId);
        Task<UserHasPlanForCreateAdver> GetUserPlan();

        Task<(bool isSuccess, string error)> ChangeAdverCreateStatus(AdminDescriptionForAdver model);
        Task<(AdverNotification adverNotification, string error)> GetAdverNotificationForUser();
        Task<(AdverNotification adverNotification, string error)> GetAllAdverNotificationForUser();
        Task<(bool isSuccess, string error)> SeenAdverNotification(List<int> adversIds);
        //Task<(bool isSuccess, string error)> CompanyUsePlanInfo();



        #region Refrence 

        Task<AllResult<List<AllAdverForAdmin>>> GetAllAdverByFilteringForRefrence(bool? isImmediatley, AdverStatus? adverStatus, AdverCreatationStatus? adverCreatationStatus, int page, int pageSize = PublicHelper.PageSize, string RefrenceId = "");
        #endregion


    }
}
