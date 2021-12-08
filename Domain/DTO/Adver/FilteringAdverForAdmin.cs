using Domain.DTO.Response;
using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class FilteringAdverForAdmin
    {
        //int page, int pageSize = PublicHelper.PageSize
        public bool? isImmediatley { get; set; }
        public AdverStatus? adverStatus { get; set; }
        public AdverCreatationStatus? adverCreatationStatus { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; } = PublicHelper.PageSize;
    }
   
    
}
