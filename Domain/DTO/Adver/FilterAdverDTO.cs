using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class FilterAdverDTO
    {
        public string Key { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Code { get; set; }

        public TypeOfCooperation?  TypeOfCooperation { get; set; }
        public WorkExperience? WorkExperience { get; set; }
        public Salary? Salary { get; set; }
        public SpecialEmpolyee? SpecialEmpolyee { get; set; }

    }
}
