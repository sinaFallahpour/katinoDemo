using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class PlanMapper:Profile
    {
        public PlanMapper()
        {
            CreateMap<CreatePlanDTO, Plan>();
            CreateMap<EditPlanDTO, Plan>()
                .ForMember(x => x.Id, i => i.MapFrom(o => o.PlanId));
            CreateMap<Plan, LoadPlan>();
            CreateMap<Plan, AllPlanForAdmin>();
            CreateMap<Plan, AllPlanForCompany>();
            //CreateMap<Plan, PlanInfo>()
            //    .ForMember(x => x.PlanName, i => i.MapFrom(o => o.Title))
            //    .ForMember(x => x.RemainingAdversCount, i => i.MapFrom(o => o.Title))
            //    .ForMember(x => x.RemainingImmediateAdversCount, i => i.MapFrom(o => o.Title))
            //    .ForMember(x => x.RemainingDays, i => i.MapFrom(o => o.Title));

        }
    }
}
