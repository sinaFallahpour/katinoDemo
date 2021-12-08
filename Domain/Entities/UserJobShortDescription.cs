using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserJobShortDescription
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }


        //navigation propperties

        public string JobTitle { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; }

        public int ResomeId { get; set; }
        public Resome Resome { get; set; }
    }
}
