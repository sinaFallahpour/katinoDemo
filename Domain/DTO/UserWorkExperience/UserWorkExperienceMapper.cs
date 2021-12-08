using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DNTPersianUtils.Core;
using Domain.Utilities;

namespace Domain.DTO.Adver
{
    public class UserWorkExperienceMapper : Profile
    {
        public UserWorkExperienceMapper()
        {
            CreateMap<UserWorkExperienceDTO, UserWorkExperience>()
                .ForMember(x => x.StartDate, a => a.MapFrom(o => o.StartDate.ToGeorgianDateTime()))
                .ForMember(x => x.EndDate, a => a.MapFrom(o => (!string.IsNullOrEmpty(o.EndDate)) ? o.EndDate.ToGeorgianDateTime().ToString() : null));

            CreateMap<UserWorkExperience, UserWorkExperienceDTO>()
            .ForMember(x => x.StartDate, a => a.MapFrom(o => o.StartDate.ToShortPersianDateString(true)))
            .ForMember(x => x.EndDate, a => a.MapFrom(o => (o.EndDate.HasValue) ? o.EndDate.ToShortPersianDateString(true) : ""));

            CreateMap<UpdateUserWorkExperienceDTO, UserWorkExperience>()
                 .ForMember(x => x.StartDate, a => a.MapFrom(o => o.StartDate.ToGeorgianDateTime()))
                .ForMember(x => x.EndDate, a => a.MapFrom(o => o.EndDate.ToGeorgianDateTime()));

        }
    }
}
