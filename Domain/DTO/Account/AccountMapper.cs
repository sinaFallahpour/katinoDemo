using AutoMapper;
using DNTPersianUtils.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<EmployeePersonalInformationDTO, User>();
            CreateMap<User, LoadEmployeePersonalInformationDTO>()
                .ForMember(x => x.EmployeeAvatar, a => a.MapFrom(c => c.Logo))
                .ForMember(x => x.ExemptionExpirestionDate, a => a.MapFrom(c => (c.ExemptionExpirestionDate != null) ?
               c.ExemptionExpirestionDate.ToShortPersianDateString(true) : ""))
                .ForMember(x => x.ExemptionExpirestionRecieveDate, a => a.MapFrom(c => (c.ExemptionExpirestionRecieveDate != null) ?
                c.ExemptionExpirestionRecieveDate.ToShortPersianDateString(true) : ""));


            CreateMap<RefrenceRegisterDTO, User>();


        }
    }
}
