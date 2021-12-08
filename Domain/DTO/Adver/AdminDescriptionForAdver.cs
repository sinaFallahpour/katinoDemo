using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AdminDescriptionForAdver
    {
        public string AdminDescription { get; set; }
        public int AdverId { get; set; }
        public AdverCreatationStatus AdverCreatationStatus { get; set; }
    }
}
