using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ChangeAdverStatusDTO
    {
        public int AdverId { get; set; }
        public AdverStatus AdverStatus { get; set; }
    }
}
