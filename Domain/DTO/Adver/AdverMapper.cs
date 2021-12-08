using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Domain.DTO.Adver
{
    public class AdverMapper : Profile
    {
        public AdverMapper()
        {
            CreateMap<AddAdverDTO, JobAdvertisement>();
            CreateMap<EditAdverDTO, JobAdvertisement>();

            CreateMap<JobAdvertisement, AllAdver>()
                .ForMember(d => d.FeildOfActivity, a => a.MapFrom(s => s.Category.Name))
                .ForMember(i => i.CompanyName, b => b.MapFrom(c => c.Company.CompanyPersianName))
                .ForMember(i => i.IsImmediate, x => x.MapFrom(o => (o.IsImmediate) ? "فوری" : "معمولی"))
                .ForMember(i => i.IsMarked, x => x.MapFrom(o => (o.IsImmediate) ? false : false));
            CreateMap<JobAdvertisement, AllAdverForAdmin>()
               .ForMember(d => d.FeildOfActivity, a => a.MapFrom(s => s.Category.Name))
               .ForMember(i => i.CompanyName, b => b.MapFrom(c => c.Company.CompanyPersianName))
               .ForMember(i => i.IsImmediate, x => x.MapFrom(o => (o.IsImmediate) ? "فوری" : "معمولی"));

            CreateMap<JobAdvertisement, AllAdverForCurrectUser>()
              .ForMember(d => d.FeildOfActivity, a => a.MapFrom(s => s.Category.Name))
              .ForMember(i => i.CompanyName, b => b.MapFrom(c => c.Company.CompanyPersianName))
              .ForMember(i => i.IsImmediate, x => x.MapFrom(o => (o.IsImmediate) ? "فوری" : "معمولی"));

            CreateMap<JobAdvertisement, AdverDetail>()
             .ForMember(d => d.FeildOfActivity, a => a.MapFrom(s => s.Category.Name))
             .ForMember(i => i.CompanyName, b => b.MapFrom(c => c.Company.CompanyPersianName))
             .ForMember(i => i.CompanyDescription, b => b.MapFrom(c => c.Company.ShortDescription))
             .ForMember(i => i.IsImmediate, x => x.MapFrom(o => (o.IsImmediate) ? "فوری" : "معمولی"))
            .ForMember(i => i.IsMarked, x => x.MapFrom(o => (o.IsImmediate) ? false : false))
            .ForMember(i => i.IsAsignResomeToThisAdver, x => x.MapFrom(o => false));



            CreateMap<JobAdvertisement, LoadAdverDTO>();


        }
    }
}
