using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class CreateJobOpportunity
    {
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "عنوان شغلی")]
        public string Title { get; set; }

        [Display(Name = "نوع قرارداد ")]
        public WorkExperience WorkExperience { get; set; }

        [Display(Name = "شهر")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string City { get; set; }

        [Display(Name = "دسته بندی شغلی")]
        public int CategoyId { get; set; }
        

    }
}
