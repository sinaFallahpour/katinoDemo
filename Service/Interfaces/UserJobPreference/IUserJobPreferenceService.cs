using Domain;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserJobPreferenceService

    {
        Task<ListOfUserJobPreferences> GetUserJobPreferenceForCurrentUser();
        Task<UserJobPreferences> GetUserJobPreferenceById(int id);
        Task<ListOfUserJobPreferences> GetUserJobPreferenceDetailsById(int id);
        Task<(bool isSuccess, List<string> errors)> AddUserJobPreference(AddUserJobPreferencesDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateUserJobPreference(EditUserJobPreferencesDTO model);
        Task<(bool isSuccess, List<string> errors)> DeleteUserJobPreference(int id);
    }
}
