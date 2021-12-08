using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DNTPersianUtils.Core;
using Domain.Utilities;

namespace Domain.DTO.Adver
{
    public class EducationalBackgroundMapper : Profile
    {
        public EducationalBackgroundMapper()
        {
            CreateMap<EducationalBackgroundDTO, EducationalBackground>()
                .ForMember(x => x.StartDate, a => a.MapFrom(o => o.StartDate.ToGeorgianDateTime()))
                .ForMember(x => x.EndDate, a => a.MapFrom(o => (!string.IsNullOrEmpty(o.EndDate)) ? o.EndDate.ToGeorgianDateTime().ToString() : null));

            CreateMap<EducationalBackground, UpdateEducationalBackgroundDTO>()
            .ForMember(x => x.StartDate, a => a.MapFrom(o => o.StartDate.ToShortPersianDateString(true)))
            .ForMember(x => x.EndDate, a => a.MapFrom(o => (o.EndDate.HasValue) ? o.EndDate.ToShortPersianDateString(true) : ""));

            CreateMap<UpdateEducationalBackgroundDTO, EducationalBackground>()
                 .ForMember(x => x.StartDate, a => a.MapFrom(o => o.StartDate.ToGeorgianDateTime()))
                .ForMember(x => x.EndDate, a => a.MapFrom(o => o.EndDate.ToGeorgianDateTime()));

        }
    }
}
