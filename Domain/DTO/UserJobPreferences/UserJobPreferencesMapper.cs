using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Domain.DTO.Adver
{
    public class UserJobPreferencesMapper : Profile
    {
        public UserJobPreferencesMapper()
        {
            CreateMap<AddUserJobPreferencesDTO, UserJobPreferences>();
            CreateMap<UserJobPreferences, AddUserJobPreferencesDTO>();
            CreateMap<EditUserJobPreferencesDTO, UserJobPreferences>();

            CreateMap<UserJobPreferences, ListOfUserJobPreferences>();
            //.ForMember(i => i.CategoryForJobPrefence, x => x.MapFrom(o => o.UserJobPreferenceCategories
            //.Select(x => new CategoryForJobPrefence()
            //{
            //    CategoryId = x.CategoryId,
            //    CategoryName = x.Category.Name
            //})));

        }
    }
}
