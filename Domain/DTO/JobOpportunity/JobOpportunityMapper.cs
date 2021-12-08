using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Domain.DTO.Adver
{
    public class JobOpportunityMapper : Profile
    {
        public JobOpportunityMapper()
        {
            CreateMap<CreateJobOpportunity, JobOpportunity>();

        }
    }
}
